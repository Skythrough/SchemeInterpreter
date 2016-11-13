// BuiltIn -- the data structure for built-in functions

// Class BuiltIn is used for representing the value of built-in functions
// such as +.  Populate the initial environment with
// (name, new BuiltIn(name)) pairs.

// The object-oriented style for implementing built-in functions would be
// to include the C# methods for implementing a Scheme built-in in the
// BuiltIn object.  This could be done by writing one subclass of class
// BuiltIn for each built-in function and implementing the method apply
// appropriately.  This requires a large number of classes, though.
// Another alternative is to program BuiltIn.apply() in a functional
// style by writing a large if-then-else chain that tests the name of
// the function symbol.

using Parse;
using System;
using System.IO;

namespace Tree {
    public class BuiltIn : Node {
        private Node symbol; // the Ident for the built-in function

        public BuiltIn(Node s) {
			symbol = s;
		}

        public Node getSymbol() {
			return symbol;
		}

        // TODO: The method isProcedure() should be defined in
        // class Node to return false.
        public override bool isProcedure() {
			return true;
		}

        public override void print(int n) {
            // there got to be a more efficient way to print n spaces
            for (int i = 0; i < n; i++)
                Console.Write(' ');
            Console.Write("#{Built-in Procedure ");
            if (symbol != null)
                symbol.print(-Math.Abs(n));
            Console.Write('}');
            if (n >= 0)
                Console.WriteLine();
        }

        // TODO: The method apply() should be defined in class Node
        // to report an error.  It should be overridden only in classes
        // BuiltIn and Closure.
        public override Node apply (Node args) {
			int num = Begin.expLength(args);
			if (num > 2) {
				Console.Error.WriteLine("error: wrong number of arguments");
			}
			if (num == 0) {
				return zeroArgs();
			}
			if (num == 1) {
				return oneArgs(args.getCar());
			}
			return twoArgs(args.getCar(), args.getCdr().getCar());
		}

		// applying zero arguments
		private Node zeroArgs() {
			string name = symbol.getName();
			if (name.Equals("read")) {
				Scanner scanner = new Scanner(Console.In);
				Parser parser = new Parser(scanner, new TreeBuilder());
				Node node = (Node) parser.parseExp();
				if (node != null) {
					return node;
				}
				return new Ident("end of the file");
			}
			else {
				if (name.Equals("newline")) {
					Console.WriteLine();
					return StringLit.getUnspecific();
				}
				if (name.Equals("interaction-environment")) {
					return Scheme4101.env;
				}
				Console.Error.WriteLine("error: wrong number of arguments");
				return Nil.getInstance();
			}
		}

		// applying one argument
		private Node oneArgs(Node arg) {
			string name = symbol.getName();
			if (name.Equals("car")) {
				return arg.getCar();
			}
			if (name.Equals("cdr")) {
				return arg.getCdr();
			}
			if (name.Equals("number?")) {
				return BoolLit.getInstance(arg.isNumber());
			}
			if (name.Equals("symbol?")) {
				return BoolLit.getInstance(arg.isSymbol());
			}
			if (name.Equals("null?")) {
				return BoolLit.getInstance(arg.isNull());
			}
			if (name.Equals("pair?")) {
				return BoolLit.getInstance(arg.isPair());
			}
			if (name.Equals("procedure?")) {
				return BoolLit.getInstance(arg.isProcedure());
			}
			if (name.Equals("write")) {
				arg.print(-1);
				return StringLit.getUnspecific();
			}
			if (name.Equals("display")) {
				StringLit.printDoubleQuotes = false;
				arg.print(-1);
				StringLit.printDoubleQuotes = true;
				return StringLit.getUnspecific();
			}
			if (!name.Equals("load")) {
				Console.Error.WriteLine("error: wrong number of arguments");
				return Nil.getInstance();
			}
			if (!arg.isString()) {
				Console.Error.WriteLine("error: wrong type of arguments");
				return Nil.getInstance();
			}
			string stringVal = arg.getStringVal();
			try {
				Scanner scanner = new Scanner(File.OpenText(stringVal));
				TreeBuilder builder = new TreeBuilder();
				Parser parser = new Parser(scanner, builder);
				for (Node node = (Node)parser.parseExp(); node != null; node = (Node)parser.parseExp()) {
					node.eval(Scheme4101.env);
				}
			}
			catch (SystemException) {
				Console.Error.WriteLine("Could not find file " + stringVal);
			}
			return StringLit.getUnspecific();
		}

		private Node twoArgs(Node arg1, Node arg2) {
			string name = this.symbol.getName();
			if (name.Equals("eq?")) {
				if (arg1.isSymbol() && arg2.isSymbol()) {
					string name2 = arg1.getName();
					string name3 = arg2.getName();
					return BoolLit.getInstance(name2.Equals(name3));
				}
				return BoolLit.getInstance(arg1 == arg2);
			}
			else {
				if (name.Equals("cons")) {
					return new Cons(arg1, arg2);
				}
				if (name.Equals("set-car!")) {
					arg1.setCar(arg2);
					return StringLit.getUnspecific();
				}
				if (name.Equals("set-cdr!")) {
					arg1.setCdr(arg2);
					return StringLit.getUnspecific();
				}
				if (name.Equals("eval")) {
					if (arg2.isEnvironment()) {
						return arg1.eval((Environment)arg2);
					}
					Console.Error.WriteLine("error: wrong type of argument");
					return Nil.getInstance();
				}
				else {
					if (name.Equals("apply")) {
						return arg1.apply(arg2);
					}
					if (name[0] != 'b' || name.Length != 2) {
						Console.Error.WriteLine("error: wrong number of arguments");
						return Nil.getInstance();
					}
					if (arg1.isNumber() && arg2.isNumber()) {
						return this.twoArgs(arg1.getIntVal(), arg2.getIntVal());
					}
					Console.Error.WriteLine("error: invalid arguments");
					return Nil.getInstance();
				}
			}
		}

		private Node twoArgs(int arg1, int arg2) {
			string name = this.symbol.getName();
			if (name.Equals("b+")) {
				return new IntLit(arg1 + arg2);
			}
			if (name.Equals("b-")) {
				return new IntLit(arg1 - arg2);
			}
			if (name.Equals("b*")) {
				return new IntLit(arg1 * arg2);
			}
			if (name.Equals("b/")) {
				return new IntLit(arg1 / arg2);
			}
			if (name.Equals("b=")) {
				return BoolLit.getInstance(arg1 == arg2);
			}
			if (name.Equals("b<")) {
				return BoolLit.getInstance(arg1 < arg2);
			}
			Console.Error.WriteLine("Error: unknown built-in function");
			return Nil.getInstance();
		}
	}
}
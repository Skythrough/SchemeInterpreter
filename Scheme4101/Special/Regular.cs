// Regular -- Parse tree node strategy for printing regular lists

using System;

namespace Tree {
    public class Regular : Special {
        public Regular() { }

		public static Node mapeval(Node exp, Environment env) {
			if (exp.isNull()) {
				return Nil.getInstance();
			}
			return new Cons(exp.getCar().eval(env), mapeval(exp.getCdr(), env));
		}

		public override Node eval(Node exp, Environment env) {
			int num = Begin.expLength(exp);
			if (num < 1) {
				Console.Error.WriteLine("error: invalid expression");
				return Nil.getInstance();
			}
			Node node = exp.getCar().eval(env);
			Node args = mapeval(exp.getCdr(), env);
			return node.apply(args);
		}

        public override void print(Node t, int n, bool p) {
            Printer.printRegular(t, n, p);
        }
    }
}
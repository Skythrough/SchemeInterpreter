// Define -- Parse tree node strategy for printing the special form define

using System;

namespace Tree {
    public class Define : Special {
		public Define() { }

		private bool check(Node parms) {
			return parms.isNull() || parms.isSymbol() || (parms.isPair() && parms.getCar().isSymbol() && check(parms.getCdr()));
		}

		public override Node eval(Node exp, Environment env) {
			int num = Begin.expLength(exp);
			if (num < 3) {
				Console.Error.WriteLine("error: invalid expression");
				return Nil.getInstance();
			}
			Node cadr = exp.getCdr().getCar();
			if (cadr.isSymbol() && num == 3) {
				Node caddr = exp.getCdr().getCdr().getCar();
				env.define(cadr, caddr.eval(env));
				return Void.getInstance();
			}
			if (!cadr.isPair()) {
				Console.Error.WriteLine("error: invalid expression");
				return Nil.getInstance();
			}
			Node caadr = cadr.getCar();
			Node cdadr = cadr.getCdr();
			Node cddr = exp.getCdr().getCdr();
			if (!caadr.isSymbol() || !check(cdadr)) {
				Console.Error.WriteLine("error: ill-formed definition");
				return Nil.getInstance();
			}
			Node node = new Cons(new Ident("lambda"), new Cons(cdadr, cddr));
			env.define(caadr, node.eval(env));
			return Void.getInstance();
		}
	
        public override void print(Node t, int n, bool p) {
            Printer.printDefine(t, n, p);
        }
    }
}
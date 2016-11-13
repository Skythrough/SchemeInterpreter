// Begin -- Parse tree node strategy for printing the special form begin

using System;

namespace Tree {
    public class Begin : Special {
		public Begin() { }

		public static int expLength(Node exp) {
			if (exp.isNull()) {
				return 0;
			}
			if (!exp.isPair()) {
				return -1;
			}
			int num = expLength(exp.getCdr());
			if (num == -1) {
				return -1;
			}
			return num + 1;
		}

		public static Node begin(Node exp, Environment env) {
			Node result = exp.getCar().eval(env);
			Node cdr = exp.getCdr();
			if (cdr.isNull()) {
				return result;
			}
			return begin(cdr, env);
		}

		public override Node eval(Node exp, Environment env) {
			int num = expLength(exp);
			if (num < 2) {
				Console.Error.WriteLine("error: invalid expression");
				return Nil.getInstance();
			}
			return begin(exp.getCdr(), env);
		}

		public override void print(Node t, int n, bool p) {
			Printer.printBegin(t, n, p);
       	}
    }
}
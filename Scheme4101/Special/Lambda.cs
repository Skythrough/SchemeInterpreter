// Lambda -- Parse tree node strategy for printing the special form lambda

using System;

namespace Tree {
    public class Lambda : Special {
		public Lambda() { }

		public override Node eval(Node exp, Environment env) {
			return new Closure(exp, env);
		}

        public override void print(Node t, int n, bool p) {
            Printer.printLambda(t, n, p);
  		}
    }
}
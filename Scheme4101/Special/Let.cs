// Let -- Parse tree node strategy for printing the special form let

using System;

namespace Tree {
    public class Let : Special {
		public Let() { }

		private static int define(Node bind, Environment env, Environment lenv) {
			if (bind.isNull()) {
				return 0;
			}
			Node car = bind.getCar();
			if (Begin.expLength(car) != 2) {
				return -1;
			}
			Node caar = car.getCar();
			Node value = car.getCdr().getCar().eval(env);
			lenv.define(caar, value);
			return Let.define(bind.getCdr(), env, lenv);
		}

		public override Node eval(Node exp, Environment env) {
			int num = Begin.expLength(exp);
			if (num < 3) {
				Console.Error.WriteLine("error: invalid expression");
				return Nil.getInstance();
			}
			Node cdar = exp.getCdr().getCar();
			Node cddr = exp.getCdr().getCdr();
			num = Begin.expLength(cdar);
			if (num < 1) {
				Console.Error.WriteLine("error: invalid expression");
				return Nil.getInstance();
			}
			Environment environment = new Environment(env);
			if (Let.define(cdar, env, environment) < 0) {
				Console.Error.WriteLine("error: invalid expression");
				return Nil.getInstance();
			}
			return Begin.begin(cddr, environment);
		}

        public override void print(Node t, int n, bool p) {
            Printer.printLet(t, n, p);
        }
    }
}
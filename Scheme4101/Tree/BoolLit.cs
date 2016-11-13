// BoolLit -- Parse tree node class for representing boolean literals

using System;

namespace Tree {
    public class BoolLit : Node {
        private bool boolVal; // do we even need this??
  
        private static BoolLit trueInstance =  new BoolLit(true);
        private static BoolLit falseInstance = new BoolLit(false);

        private BoolLit(bool b) {
            boolVal = b;
        }
  
        public static BoolLit getInstance(bool val) {
            if (val)
                return trueInstance;
            else
                return falseInstance;
        }

		public override Node eval(Environment env) {
			return this;
		}

        public override void print(int n) {
            Printer.printBoolLit(n, boolVal);
        }

        public override bool isBool() {
            return true;
        }
    }
}
// StringLit -- Parse tree node class for representing string literals

using System;

namespace Tree {
    public class StringLit : Node {
        private string stringVal;


		private static StringLit unspecific = new StringLit("#{Unspecific}");

		public static bool printDoubleQuotes = true;

        public StringLit(string s) {
            stringVal = s;
        }

		public override string getStringVal() {
			return stringVal;
		}

		public override Node eval(Environment env) {
			return this;
		}

        public override void print(int n) {
			if (StringLit.printDoubleQuotes && !stringVal.Equals("#{Unspecific}")) {
            	Printer.printStringLit(n, stringVal);
			}
			else {
				for (int i = 0; i < n; i++) {
					Console.Write(' ');
				}
				Console.WriteLine(stringVal);
				if (n >= 0) {
					Console.WriteLine();
				}
			}
        }

        public override bool isString() {
            return true;
        }

		public static Node getUnspecific() {
			return unspecific;
		}
    }
}
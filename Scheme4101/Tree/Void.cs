using System;

namespace Tree {
	public class Void : Node {
		public Void() { }

		private static Void empty = new Void();

		public static Void getInstance() {
			return empty;
		}

		public override void print(int n) { }
	}
}
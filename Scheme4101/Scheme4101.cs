// @author Wilson Zhu
// The main program

using System;
using Parse;
using Tokens;
using Tree;
using System.IO;

public class Scheme4101 {
	public static Tree.Environment env = null;

	public static int Main(string[] args) {
		// Create scanner that reads from standard input
		Scanner scanner = new Scanner(Console.In);

		if (args.Length > 1 || (args.Length == 1 && ! args[0].Equals("-d"))) {
			Console.Error.WriteLine("Usage: mono SPP [-d]");
			return 1;
		}

		// If command line option -d is provided, debug the scanner.
		if (args.Length == 1 && args[0].Equals("-d")) {
			// Console.Write("Scheme 4101> ");
			Token tok = scanner.getNextToken();
			while (tok != null) {
				TokenType tt = tok.getType();

				Console.Write(tt);
				if (tt == TokenType.INT) {
					Console.WriteLine (", intVal = " + tok.getIntVal ());
				} else if (tt == TokenType.STRING) {
					Console.WriteLine (", stringVal = " + tok.getStringVal ());
				} else if (tt == TokenType.IDENT) {
					Console.WriteLine (", name = " + tok.getName ());
				} else {
					Console.WriteLine ();
				}

				// Console.Write("Scheme 4101> ");
				tok = scanner.getNextToken();
			}
			return 0;
		}

		// Create parser
		TreeBuilder builder = new TreeBuilder();
		Parser parser = new Parser(scanner, builder);
		Node root;

		// TODO: Create and populate the built-in environment and
		// create the top-level environment

		// built-in environment
		env = new Tree.Environment();
		string[] keywords = new string[] {
			"symbol?",
			"number?",
			"null?",
			"pair?",
			"eq?",
			"procedure?",
			"b+",
			"b-",
			"b/",
			"b=",
			"b<",
			"car",
			"cdr",
			"cons",
			"set-car!",
			"set-cdr!",
			"read",
			"write",
			"display",
			"newline",
			"eval",
			"apply",
			"interaction-environment",
			"load"
		};
		for (int i = 0; i < keywords.Length; i++) {
			string n = keywords [i];
			Ident identifier = new Ident(n);
			env.define(identifier, new BuiltIn(identifier));
		}

		// top-level environment
		env = new Tree.Environment(env);
		Console.Write("> ");

		// Read-eval-print loop
		root = (Node) parser.parseExp();
		while (root != null) {
			root.eval(env).print(0);
			Console.Write("> ");
			root = (Node) parser.parseExp();
		}
		return 0;
	}
}

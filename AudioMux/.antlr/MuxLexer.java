// Generated from /home/redacted/LoreBot/AudioMux/MuxLexer.g4 by ANTLR 4.9.2
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class MuxLexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.9.2", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		WHITESPACE=1, TRACK=2, SIGNAL=3, EXPORT=4, SEMICOLON=5, COLON=6, COMMA=7, 
		BRACKET_OPEN=8, BRACKET_CLOSE=9, IDENTIFIER=10;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"WHITESPACE", "TRACK", "SIGNAL", "EXPORT", "SEMICOLON", "COLON", "COMMA", 
			"BRACKET_OPEN", "BRACKET_CLOSE", "IDENTIFIER"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, null, "'track'", "'signal'", "'export'", "';'", "':'", "','", "'('", 
			"')'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "WHITESPACE", "TRACK", "SIGNAL", "EXPORT", "SEMICOLON", "COLON", 
			"COMMA", "BRACKET_OPEN", "BRACKET_CLOSE", "IDENTIFIER"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}


	public MuxLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "MuxLexer.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\2\fA\b\1\4\2\t\2\4"+
		"\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13\t"+
		"\13\3\2\6\2\31\n\2\r\2\16\2\32\3\2\3\2\3\3\3\3\3\3\3\3\3\3\3\3\3\4\3\4"+
		"\3\4\3\4\3\4\3\4\3\4\3\5\3\5\3\5\3\5\3\5\3\5\3\5\3\6\3\6\3\7\3\7\3\b\3"+
		"\b\3\t\3\t\3\n\3\n\3\13\6\13>\n\13\r\13\16\13?\3?\2\f\3\3\5\4\7\5\t\6"+
		"\13\7\r\b\17\t\21\n\23\13\25\f\3\2\2\2B\2\3\3\2\2\2\2\5\3\2\2\2\2\7\3"+
		"\2\2\2\2\t\3\2\2\2\2\13\3\2\2\2\2\r\3\2\2\2\2\17\3\2\2\2\2\21\3\2\2\2"+
		"\2\23\3\2\2\2\2\25\3\2\2\2\3\30\3\2\2\2\5\36\3\2\2\2\7$\3\2\2\2\t+\3\2"+
		"\2\2\13\62\3\2\2\2\r\64\3\2\2\2\17\66\3\2\2\2\218\3\2\2\2\23:\3\2\2\2"+
		"\25=\3\2\2\2\27\31\7\"\2\2\30\27\3\2\2\2\31\32\3\2\2\2\32\30\3\2\2\2\32"+
		"\33\3\2\2\2\33\34\3\2\2\2\34\35\b\2\2\2\35\4\3\2\2\2\36\37\7v\2\2\37 "+
		"\7t\2\2 !\7c\2\2!\"\7e\2\2\"#\7m\2\2#\6\3\2\2\2$%\7u\2\2%&\7k\2\2&\'\7"+
		"i\2\2\'(\7p\2\2()\7c\2\2)*\7n\2\2*\b\3\2\2\2+,\7g\2\2,-\7z\2\2-.\7r\2"+
		"\2./\7q\2\2/\60\7t\2\2\60\61\7v\2\2\61\n\3\2\2\2\62\63\7=\2\2\63\f\3\2"+
		"\2\2\64\65\7<\2\2\65\16\3\2\2\2\66\67\7.\2\2\67\20\3\2\2\289\7*\2\29\22"+
		"\3\2\2\2:;\7+\2\2;\24\3\2\2\2<>\13\2\2\2=<\3\2\2\2>?\3\2\2\2?@\3\2\2\2"+
		"?=\3\2\2\2@\26\3\2\2\2\5\2\32?\3\b\2\2";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}
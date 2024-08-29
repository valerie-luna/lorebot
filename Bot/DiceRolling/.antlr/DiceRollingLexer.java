// Generated from /home/redacted/ShadowrunWeatherBot/DiceRolling/DiceRollingLexer.g4 by ANTLR 4.9.2
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class DiceRollingLexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.9.2", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		WHITESPACE=1, SS_SHADOWRUN=2, SS_OLD_SHADOWRUN=3, SS_EARTHDAWN=4, KW_TARGET=5, 
		KW_INDEFINITE_EXPLOSION=6, KW_EXPLOSION=7, KW_UNSORT=8, COMMENT=9, OPENBRACKET=10, 
		CLOSEBRACKET=11, PLUS=12, MINUS=13, MULTIPLY=14, DIVIDE=15, SPACEDINT=16, 
		INT=17, DICE=18, ANYTHING=19;
	public static final int
		COMMENTARY=1;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE", "COMMENTARY"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"WHITESPACE", "POSDIGIT", "DIGIT", "SS_SHADOWRUN", "SS_OLD_SHADOWRUN", 
			"SS_EARTHDAWN", "KW_TARGET", "KW_INDEFINITE_EXPLOSION", "KW_EXPLOSION", 
			"KW_UNSORT", "COMMENT", "OPENBRACKET", "CLOSEBRACKET", "PLUS", "MINUS", 
			"MULTIPLY", "DIVIDE", "SPACEDINT", "INT", "DICE", "ANYTHING"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, null, "'sr'", "'osr'", "'ed'", "'t'", "'ie'", "'e'", "'unsort'", 
			"'!'", "'('", "')'", "'+'", "'-'", "'*'", "'/'", null, null, "'d'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "WHITESPACE", "SS_SHADOWRUN", "SS_OLD_SHADOWRUN", "SS_EARTHDAWN", 
			"KW_TARGET", "KW_INDEFINITE_EXPLOSION", "KW_EXPLOSION", "KW_UNSORT", 
			"COMMENT", "OPENBRACKET", "CLOSEBRACKET", "PLUS", "MINUS", "MULTIPLY", 
			"DIVIDE", "SPACEDINT", "INT", "DICE", "ANYTHING"
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


	public DiceRollingLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "DiceRollingLexer.g4"; }

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
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\2\25r\b\1\b\1\4\2\t"+
		"\2\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13"+
		"\t\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\3\2\6\2\60\n\2\r\2\16\2\61\3"+
		"\2\3\2\3\3\3\3\3\4\3\4\3\5\3\5\3\5\3\6\3\6\3\6\3\6\3\7\3\7\3\7\3\b\3\b"+
		"\3\t\3\t\3\t\3\n\3\n\3\13\3\13\3\13\3\13\3\13\3\13\3\13\3\f\3\f\3\f\3"+
		"\f\3\r\3\r\3\16\3\16\3\17\3\17\3\20\3\20\3\21\3\21\3\22\3\22\3\23\3\23"+
		"\3\23\3\24\3\24\7\24g\n\24\f\24\16\24j\13\24\3\25\3\25\3\26\6\26o\n\26"+
		"\r\26\16\26p\2\2\27\4\3\6\2\b\2\n\4\f\5\16\6\20\7\22\b\24\t\26\n\30\13"+
		"\32\f\34\r\36\16 \17\"\20$\21&\22(\23*\24,\25\4\2\3\4\3\2\63;\3\2\62;"+
		"\2q\2\4\3\2\2\2\2\n\3\2\2\2\2\f\3\2\2\2\2\16\3\2\2\2\2\20\3\2\2\2\2\22"+
		"\3\2\2\2\2\24\3\2\2\2\2\26\3\2\2\2\2\30\3\2\2\2\2\32\3\2\2\2\2\34\3\2"+
		"\2\2\2\36\3\2\2\2\2 \3\2\2\2\2\"\3\2\2\2\2$\3\2\2\2\2&\3\2\2\2\2(\3\2"+
		"\2\2\2*\3\2\2\2\3,\3\2\2\2\4/\3\2\2\2\6\65\3\2\2\2\b\67\3\2\2\2\n9\3\2"+
		"\2\2\f<\3\2\2\2\16@\3\2\2\2\20C\3\2\2\2\22E\3\2\2\2\24H\3\2\2\2\26J\3"+
		"\2\2\2\30Q\3\2\2\2\32U\3\2\2\2\34W\3\2\2\2\36Y\3\2\2\2 [\3\2\2\2\"]\3"+
		"\2\2\2$_\3\2\2\2&a\3\2\2\2(d\3\2\2\2*k\3\2\2\2,n\3\2\2\2.\60\7\"\2\2/"+
		".\3\2\2\2\60\61\3\2\2\2\61/\3\2\2\2\61\62\3\2\2\2\62\63\3\2\2\2\63\64"+
		"\b\2\2\2\64\5\3\2\2\2\65\66\t\2\2\2\66\7\3\2\2\2\678\t\3\2\28\t\3\2\2"+
		"\29:\7u\2\2:;\7t\2\2;\13\3\2\2\2<=\7q\2\2=>\7u\2\2>?\7t\2\2?\r\3\2\2\2"+
		"@A\7g\2\2AB\7f\2\2B\17\3\2\2\2CD\7v\2\2D\21\3\2\2\2EF\7k\2\2FG\7g\2\2"+
		"G\23\3\2\2\2HI\7g\2\2I\25\3\2\2\2JK\7w\2\2KL\7p\2\2LM\7u\2\2MN\7q\2\2"+
		"NO\7t\2\2OP\7v\2\2P\27\3\2\2\2QR\7#\2\2RS\3\2\2\2ST\b\f\3\2T\31\3\2\2"+
		"\2UV\7*\2\2V\33\3\2\2\2WX\7+\2\2X\35\3\2\2\2YZ\7-\2\2Z\37\3\2\2\2[\\\7"+
		"/\2\2\\!\3\2\2\2]^\7,\2\2^#\3\2\2\2_`\7\61\2\2`%\3\2\2\2ab\5(\24\2bc\7"+
		"\"\2\2c\'\3\2\2\2dh\5\6\3\2eg\5\b\4\2fe\3\2\2\2gj\3\2\2\2hf\3\2\2\2hi"+
		"\3\2\2\2i)\3\2\2\2jh\3\2\2\2kl\7f\2\2l+\3\2\2\2mo\13\2\2\2nm\3\2\2\2o"+
		"p\3\2\2\2pn\3\2\2\2pq\3\2\2\2q-\3\2\2\2\7\2\3\61hp\4\b\2\2\4\3\2";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}
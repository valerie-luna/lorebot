// Generated from /home/redacted/LoreBot/AudioMux/MuxParser.g4 by ANTLR 4.9.2
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class MuxParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.9.2", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		WHITESPACE=1, TRACK=2, SIGNAL=3, EXPORT=4, SEMICOLON=5, COLON=6, COMMA=7, 
		BRACKET_OPEN=8, BRACKET_CLOSE=9, IDENTIFIER=10;
	public static final int
		RULE_statement = 0, RULE_track = 1, RULE_signal = 2, RULE_export = 3, 
		RULE_expression = 4, RULE_func = 5, RULE_arguments = 6, RULE_sigSpec = 7;
	private static String[] makeRuleNames() {
		return new String[] {
			"statement", "track", "signal", "export", "expression", "func", "arguments", 
			"sigSpec"
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

	@Override
	public String getGrammarFileName() { return "MuxParser.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public MuxParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	public static class StatementContext extends ParserRuleContext {
		public TerminalNode SEMICOLON() { return getToken(MuxParser.SEMICOLON, 0); }
		public TrackContext track() {
			return getRuleContext(TrackContext.class,0);
		}
		public SignalContext signal() {
			return getRuleContext(SignalContext.class,0);
		}
		public ExportContext export() {
			return getRuleContext(ExportContext.class,0);
		}
		public StatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement; }
	}

	public final StatementContext statement() throws RecognitionException {
		StatementContext _localctx = new StatementContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_statement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(19);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case TRACK:
				{
				setState(16);
				track();
				}
				break;
			case SIGNAL:
				{
				setState(17);
				signal();
				}
				break;
			case EXPORT:
				{
				setState(18);
				export();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(21);
			match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TrackContext extends ParserRuleContext {
		public TerminalNode TRACK() { return getToken(MuxParser.TRACK, 0); }
		public TerminalNode IDENTIFIER() { return getToken(MuxParser.IDENTIFIER, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TrackContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_track; }
	}

	public final TrackContext track() throws RecognitionException {
		TrackContext _localctx = new TrackContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_track);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(23);
			match(TRACK);
			setState(24);
			match(IDENTIFIER);
			setState(25);
			expression();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class SignalContext extends ParserRuleContext {
		public TerminalNode SIGNAL() { return getToken(MuxParser.SIGNAL, 0); }
		public TerminalNode IDENTIFIER() { return getToken(MuxParser.IDENTIFIER, 0); }
		public SignalContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_signal; }
	}

	public final SignalContext signal() throws RecognitionException {
		SignalContext _localctx = new SignalContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_signal);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(27);
			match(SIGNAL);
			setState(28);
			match(IDENTIFIER);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ExportContext extends ParserRuleContext {
		public TerminalNode EXPORT() { return getToken(MuxParser.EXPORT, 0); }
		public TerminalNode IDENTIFIER() { return getToken(MuxParser.IDENTIFIER, 0); }
		public ExportContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_export; }
	}

	public final ExportContext export() throws RecognitionException {
		ExportContext _localctx = new ExportContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_export);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(30);
			match(EXPORT);
			setState(31);
			match(IDENTIFIER);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ExpressionContext extends ParserRuleContext {
		public TerminalNode IDENTIFIER() { return getToken(MuxParser.IDENTIFIER, 0); }
		public FuncContext func() {
			return getRuleContext(FuncContext.class,0);
		}
		public ExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expression; }
	}

	public final ExpressionContext expression() throws RecognitionException {
		ExpressionContext _localctx = new ExpressionContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_expression);
		try {
			setState(35);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,1,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(33);
				match(IDENTIFIER);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(34);
				func();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class FuncContext extends ParserRuleContext {
		public TerminalNode IDENTIFIER() { return getToken(MuxParser.IDENTIFIER, 0); }
		public TerminalNode BRACKET_OPEN() { return getToken(MuxParser.BRACKET_OPEN, 0); }
		public ArgumentsContext arguments() {
			return getRuleContext(ArgumentsContext.class,0);
		}
		public TerminalNode BRACKET_CLOSE() { return getToken(MuxParser.BRACKET_CLOSE, 0); }
		public SigSpecContext sigSpec() {
			return getRuleContext(SigSpecContext.class,0);
		}
		public FuncContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_func; }
	}

	public final FuncContext func() throws RecognitionException {
		FuncContext _localctx = new FuncContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_func);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(37);
			match(IDENTIFIER);
			setState(38);
			match(BRACKET_OPEN);
			setState(39);
			arguments();
			setState(41);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COLON) {
				{
				setState(40);
				sigSpec();
				}
			}

			setState(43);
			match(BRACKET_CLOSE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ArgumentsContext extends ParserRuleContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode COMMA() { return getToken(MuxParser.COMMA, 0); }
		public ArgumentsContext arguments() {
			return getRuleContext(ArgumentsContext.class,0);
		}
		public ArgumentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arguments; }
	}

	public final ArgumentsContext arguments() throws RecognitionException {
		ArgumentsContext _localctx = new ArgumentsContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_arguments);
		try {
			setState(50);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,3,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(45);
				expression();
				setState(46);
				match(COMMA);
				setState(47);
				arguments();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(49);
				expression();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class SigSpecContext extends ParserRuleContext {
		public TerminalNode COLON() { return getToken(MuxParser.COLON, 0); }
		public TerminalNode IDENTIFIER() { return getToken(MuxParser.IDENTIFIER, 0); }
		public SigSpecContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_sigSpec; }
	}

	public final SigSpecContext sigSpec() throws RecognitionException {
		SigSpecContext _localctx = new SigSpecContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_sigSpec);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(52);
			match(COLON);
			setState(53);
			match(IDENTIFIER);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3\f:\4\2\t\2\4\3\t"+
		"\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\3\2\3\2\3\2\5\2\26"+
		"\n\2\3\2\3\2\3\3\3\3\3\3\3\3\3\4\3\4\3\4\3\5\3\5\3\5\3\6\3\6\5\6&\n\6"+
		"\3\7\3\7\3\7\3\7\5\7,\n\7\3\7\3\7\3\b\3\b\3\b\3\b\3\b\5\b\65\n\b\3\t\3"+
		"\t\3\t\3\t\2\2\n\2\4\6\b\n\f\16\20\2\2\2\66\2\25\3\2\2\2\4\31\3\2\2\2"+
		"\6\35\3\2\2\2\b \3\2\2\2\n%\3\2\2\2\f\'\3\2\2\2\16\64\3\2\2\2\20\66\3"+
		"\2\2\2\22\26\5\4\3\2\23\26\5\6\4\2\24\26\5\b\5\2\25\22\3\2\2\2\25\23\3"+
		"\2\2\2\25\24\3\2\2\2\26\27\3\2\2\2\27\30\7\7\2\2\30\3\3\2\2\2\31\32\7"+
		"\4\2\2\32\33\7\f\2\2\33\34\5\n\6\2\34\5\3\2\2\2\35\36\7\5\2\2\36\37\7"+
		"\f\2\2\37\7\3\2\2\2 !\7\6\2\2!\"\7\f\2\2\"\t\3\2\2\2#&\7\f\2\2$&\5\f\7"+
		"\2%#\3\2\2\2%$\3\2\2\2&\13\3\2\2\2\'(\7\f\2\2()\7\n\2\2)+\5\16\b\2*,\5"+
		"\20\t\2+*\3\2\2\2+,\3\2\2\2,-\3\2\2\2-.\7\13\2\2.\r\3\2\2\2/\60\5\n\6"+
		"\2\60\61\7\t\2\2\61\62\5\16\b\2\62\65\3\2\2\2\63\65\5\n\6\2\64/\3\2\2"+
		"\2\64\63\3\2\2\2\65\17\3\2\2\2\66\67\7\b\2\2\678\7\f\2\28\21\3\2\2\2\6"+
		"\25%+\64";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}
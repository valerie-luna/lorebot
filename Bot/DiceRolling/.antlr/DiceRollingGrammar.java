// Generated from /home/redacted/ShadowrunWeatherBot/DiceRolling/DiceRollingGrammar.g4 by ANTLR 4.9.2
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class DiceRollingGrammar extends Parser {
	static { RuntimeMetaData.checkVersion("4.9.2", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		WHITESPACE=1, KW_TARGET=2, KW_INDEFINITE_EXPLOSION=3, KW_TOTALLING_EXPLOSION=4, 
		KW_EXPLOSION=5, KW_UNSORT=6, COMMENT=7, OPENBRACKET=8, CLOSEBRACKET=9, 
		PLUS=10, MINUS=11, INT=12, DICE=13, ANYTHING=14;
	public static final int
		RULE_diceRollRequest = 0, RULE_reason = 1, RULE_multiplier = 2, RULE_arithmetic = 3, 
		RULE_expr = 4, RULE_dice = 5, RULE_diceModifiers = 6, RULE_rawDice = 7, 
		RULE_integer = 8, RULE_sign = 9;
	private static String[] makeRuleNames() {
		return new String[] {
			"diceRollRequest", "reason", "multiplier", "arithmetic", "expr", "dice", 
			"diceModifiers", "rawDice", "integer", "sign"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, null, "'t'", "'ie'", "'te'", "'e'", "'unsort'", "'!'", "'('", "')'", 
			"'+'", "'-'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "WHITESPACE", "KW_TARGET", "KW_INDEFINITE_EXPLOSION", "KW_TOTALLING_EXPLOSION", 
			"KW_EXPLOSION", "KW_UNSORT", "COMMENT", "OPENBRACKET", "CLOSEBRACKET", 
			"PLUS", "MINUS", "INT", "DICE", "ANYTHING"
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
	public String getGrammarFileName() { return "DiceRollingGrammar.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public DiceRollingGrammar(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	public static class DiceRollRequestContext extends ParserRuleContext {
		public ArithmeticContext arithmetic() {
			return getRuleContext(ArithmeticContext.class,0);
		}
		public MultiplierContext multiplier() {
			return getRuleContext(MultiplierContext.class,0);
		}
		public ReasonContext reason() {
			return getRuleContext(ReasonContext.class,0);
		}
		public DiceRollRequestContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_diceRollRequest; }
	}

	public final DiceRollRequestContext diceRollRequest() throws RecognitionException {
		DiceRollRequestContext _localctx = new DiceRollRequestContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_diceRollRequest);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(21);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,0,_ctx) ) {
			case 1:
				{
				setState(20);
				multiplier();
				}
				break;
			}
			setState(23);
			arithmetic(0);
			setState(25);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMENT) {
				{
				setState(24);
				reason();
				}
			}

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

	public static class ReasonContext extends ParserRuleContext {
		public TerminalNode COMMENT() { return getToken(DiceRollingGrammar.COMMENT, 0); }
		public TerminalNode ANYTHING() { return getToken(DiceRollingGrammar.ANYTHING, 0); }
		public ReasonContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_reason; }
	}

	public final ReasonContext reason() throws RecognitionException {
		ReasonContext _localctx = new ReasonContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_reason);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(27);
			match(COMMENT);
			setState(28);
			match(ANYTHING);
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

	public static class MultiplierContext extends ParserRuleContext {
		public TerminalNode INT() { return getToken(DiceRollingGrammar.INT, 0); }
		public MultiplierContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_multiplier; }
	}

	public final MultiplierContext multiplier() throws RecognitionException {
		MultiplierContext _localctx = new MultiplierContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_multiplier);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(30);
			match(INT);
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

	public static class ArithmeticContext extends ParserRuleContext {
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public ArithmeticContext arithmetic() {
			return getRuleContext(ArithmeticContext.class,0);
		}
		public TerminalNode PLUS() { return getToken(DiceRollingGrammar.PLUS, 0); }
		public TerminalNode MINUS() { return getToken(DiceRollingGrammar.MINUS, 0); }
		public ArithmeticContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arithmetic; }
	}

	public final ArithmeticContext arithmetic() throws RecognitionException {
		return arithmetic(0);
	}

	private ArithmeticContext arithmetic(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ArithmeticContext _localctx = new ArithmeticContext(_ctx, _parentState);
		ArithmeticContext _prevctx = _localctx;
		int _startState = 6;
		enterRecursionRule(_localctx, 6, RULE_arithmetic, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(33);
			expr();
			}
			_ctx.stop = _input.LT(-1);
			setState(43);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,3,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(41);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,2,_ctx) ) {
					case 1:
						{
						_localctx = new ArithmeticContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_arithmetic);
						setState(35);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(36);
						match(PLUS);
						setState(37);
						expr();
						}
						break;
					case 2:
						{
						_localctx = new ArithmeticContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_arithmetic);
						setState(38);
						if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
						setState(39);
						match(MINUS);
						setState(40);
						expr();
						}
						break;
					}
					} 
				}
				setState(45);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,3,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public static class ExprContext extends ParserRuleContext {
		public DiceContext dice() {
			return getRuleContext(DiceContext.class,0);
		}
		public IntegerContext integer() {
			return getRuleContext(IntegerContext.class,0);
		}
		public ExprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expr; }
	}

	public final ExprContext expr() throws RecognitionException {
		ExprContext _localctx = new ExprContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_expr);
		try {
			setState(48);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,4,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(46);
				dice();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(47);
				integer();
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

	public static class DiceContext extends ParserRuleContext {
		public RawDiceContext rawDice() {
			return getRuleContext(RawDiceContext.class,0);
		}
		public List<DiceModifiersContext> diceModifiers() {
			return getRuleContexts(DiceModifiersContext.class);
		}
		public DiceModifiersContext diceModifiers(int i) {
			return getRuleContext(DiceModifiersContext.class,i);
		}
		public TerminalNode OPENBRACKET() { return getToken(DiceRollingGrammar.OPENBRACKET, 0); }
		public TerminalNode CLOSEBRACKET() { return getToken(DiceRollingGrammar.CLOSEBRACKET, 0); }
		public DiceContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_dice; }
	}

	public final DiceContext dice() throws RecognitionException {
		DiceContext _localctx = new DiceContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_dice);
		int _la;
		try {
			int _alt;
			setState(67);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case INT:
				enterOuterAlt(_localctx, 1);
				{
				setState(50);
				rawDice();
				setState(54);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,5,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(51);
						diceModifiers();
						}
						} 
					}
					setState(56);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,5,_ctx);
				}
				}
				break;
			case OPENBRACKET:
				enterOuterAlt(_localctx, 2);
				{
				setState(57);
				match(OPENBRACKET);
				setState(58);
				rawDice();
				setState(62);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << KW_TARGET) | (1L << KW_INDEFINITE_EXPLOSION) | (1L << KW_TOTALLING_EXPLOSION) | (1L << KW_EXPLOSION) | (1L << KW_UNSORT))) != 0)) {
					{
					{
					setState(59);
					diceModifiers();
					}
					}
					setState(64);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(65);
				match(CLOSEBRACKET);
				}
				break;
			default:
				throw new NoViableAltException(this);
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

	public static class DiceModifiersContext extends ParserRuleContext {
		public DiceModifiersContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_diceModifiers; }
	 
		public DiceModifiersContext() { }
		public void copyFrom(DiceModifiersContext ctx) {
			super.copyFrom(ctx);
		}
	}
	public static class DMExplosionContext extends DiceModifiersContext {
		public TerminalNode KW_EXPLOSION() { return getToken(DiceRollingGrammar.KW_EXPLOSION, 0); }
		public TerminalNode INT() { return getToken(DiceRollingGrammar.INT, 0); }
		public SignContext sign() {
			return getRuleContext(SignContext.class,0);
		}
		public DMExplosionContext(DiceModifiersContext ctx) { copyFrom(ctx); }
	}
	public static class DMTargetContext extends DiceModifiersContext {
		public TerminalNode KW_TARGET() { return getToken(DiceRollingGrammar.KW_TARGET, 0); }
		public TerminalNode INT() { return getToken(DiceRollingGrammar.INT, 0); }
		public SignContext sign() {
			return getRuleContext(SignContext.class,0);
		}
		public DMTargetContext(DiceModifiersContext ctx) { copyFrom(ctx); }
	}
	public static class DMIndefiniteContext extends DiceModifiersContext {
		public TerminalNode KW_INDEFINITE_EXPLOSION() { return getToken(DiceRollingGrammar.KW_INDEFINITE_EXPLOSION, 0); }
		public TerminalNode INT() { return getToken(DiceRollingGrammar.INT, 0); }
		public SignContext sign() {
			return getRuleContext(SignContext.class,0);
		}
		public DMIndefiniteContext(DiceModifiersContext ctx) { copyFrom(ctx); }
	}
	public static class DMTotallingContext extends DiceModifiersContext {
		public TerminalNode KW_TOTALLING_EXPLOSION() { return getToken(DiceRollingGrammar.KW_TOTALLING_EXPLOSION, 0); }
		public DMTotallingContext(DiceModifiersContext ctx) { copyFrom(ctx); }
	}
	public static class DMUnsortContext extends DiceModifiersContext {
		public TerminalNode KW_UNSORT() { return getToken(DiceRollingGrammar.KW_UNSORT, 0); }
		public DMUnsortContext(DiceModifiersContext ctx) { copyFrom(ctx); }
	}

	public final DiceModifiersContext diceModifiers() throws RecognitionException {
		DiceModifiersContext _localctx = new DiceModifiersContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_diceModifiers);
		try {
			setState(86);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_TARGET:
				_localctx = new DMTargetContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(69);
				match(KW_TARGET);
				setState(70);
				match(INT);
				setState(72);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,8,_ctx) ) {
				case 1:
					{
					setState(71);
					sign();
					}
					break;
				}
				}
				break;
			case KW_EXPLOSION:
				_localctx = new DMExplosionContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(74);
				match(KW_EXPLOSION);
				setState(75);
				match(INT);
				setState(77);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,9,_ctx) ) {
				case 1:
					{
					setState(76);
					sign();
					}
					break;
				}
				}
				break;
			case KW_INDEFINITE_EXPLOSION:
				_localctx = new DMIndefiniteContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(79);
				match(KW_INDEFINITE_EXPLOSION);
				setState(80);
				match(INT);
				setState(82);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,10,_ctx) ) {
				case 1:
					{
					setState(81);
					sign();
					}
					break;
				}
				}
				break;
			case KW_TOTALLING_EXPLOSION:
				_localctx = new DMTotallingContext(_localctx);
				enterOuterAlt(_localctx, 4);
				{
				setState(84);
				match(KW_TOTALLING_EXPLOSION);
				}
				break;
			case KW_UNSORT:
				_localctx = new DMUnsortContext(_localctx);
				enterOuterAlt(_localctx, 5);
				{
				setState(85);
				match(KW_UNSORT);
				}
				break;
			default:
				throw new NoViableAltException(this);
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

	public static class RawDiceContext extends ParserRuleContext {
		public List<TerminalNode> INT() { return getTokens(DiceRollingGrammar.INT); }
		public TerminalNode INT(int i) {
			return getToken(DiceRollingGrammar.INT, i);
		}
		public TerminalNode DICE() { return getToken(DiceRollingGrammar.DICE, 0); }
		public RawDiceContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_rawDice; }
	}

	public final RawDiceContext rawDice() throws RecognitionException {
		RawDiceContext _localctx = new RawDiceContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_rawDice);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(88);
			match(INT);
			setState(89);
			match(DICE);
			setState(90);
			match(INT);
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

	public static class IntegerContext extends ParserRuleContext {
		public TerminalNode INT() { return getToken(DiceRollingGrammar.INT, 0); }
		public IntegerContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_integer; }
	}

	public final IntegerContext integer() throws RecognitionException {
		IntegerContext _localctx = new IntegerContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_integer);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(92);
			match(INT);
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

	public static class SignContext extends ParserRuleContext {
		public TerminalNode PLUS() { return getToken(DiceRollingGrammar.PLUS, 0); }
		public TerminalNode MINUS() { return getToken(DiceRollingGrammar.MINUS, 0); }
		public SignContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_sign; }
	}

	public final SignContext sign() throws RecognitionException {
		SignContext _localctx = new SignContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_sign);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(94);
			_la = _input.LA(1);
			if ( !(_la==PLUS || _la==MINUS) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
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

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 3:
			return arithmetic_sempred((ArithmeticContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean arithmetic_sempred(ArithmeticContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 2);
		case 1:
			return precpred(_ctx, 1);
		}
		return true;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3\20c\4\2\t\2\4\3\t"+
		"\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13\t\13\3"+
		"\2\5\2\30\n\2\3\2\3\2\5\2\34\n\2\3\3\3\3\3\3\3\4\3\4\3\5\3\5\3\5\3\5\3"+
		"\5\3\5\3\5\3\5\3\5\7\5,\n\5\f\5\16\5/\13\5\3\6\3\6\5\6\63\n\6\3\7\3\7"+
		"\7\7\67\n\7\f\7\16\7:\13\7\3\7\3\7\3\7\7\7?\n\7\f\7\16\7B\13\7\3\7\3\7"+
		"\5\7F\n\7\3\b\3\b\3\b\5\bK\n\b\3\b\3\b\3\b\5\bP\n\b\3\b\3\b\3\b\5\bU\n"+
		"\b\3\b\3\b\5\bY\n\b\3\t\3\t\3\t\3\t\3\n\3\n\3\13\3\13\3\13\2\3\b\f\2\4"+
		"\6\b\n\f\16\20\22\24\2\3\3\2\f\r\2g\2\27\3\2\2\2\4\35\3\2\2\2\6 \3\2\2"+
		"\2\b\"\3\2\2\2\n\62\3\2\2\2\fE\3\2\2\2\16X\3\2\2\2\20Z\3\2\2\2\22^\3\2"+
		"\2\2\24`\3\2\2\2\26\30\5\6\4\2\27\26\3\2\2\2\27\30\3\2\2\2\30\31\3\2\2"+
		"\2\31\33\5\b\5\2\32\34\5\4\3\2\33\32\3\2\2\2\33\34\3\2\2\2\34\3\3\2\2"+
		"\2\35\36\7\t\2\2\36\37\7\20\2\2\37\5\3\2\2\2 !\7\16\2\2!\7\3\2\2\2\"#"+
		"\b\5\1\2#$\5\n\6\2$-\3\2\2\2%&\f\4\2\2&\'\7\f\2\2\',\5\n\6\2()\f\3\2\2"+
		")*\7\r\2\2*,\5\n\6\2+%\3\2\2\2+(\3\2\2\2,/\3\2\2\2-+\3\2\2\2-.\3\2\2\2"+
		".\t\3\2\2\2/-\3\2\2\2\60\63\5\f\7\2\61\63\5\22\n\2\62\60\3\2\2\2\62\61"+
		"\3\2\2\2\63\13\3\2\2\2\648\5\20\t\2\65\67\5\16\b\2\66\65\3\2\2\2\67:\3"+
		"\2\2\28\66\3\2\2\289\3\2\2\29F\3\2\2\2:8\3\2\2\2;<\7\n\2\2<@\5\20\t\2"+
		"=?\5\16\b\2>=\3\2\2\2?B\3\2\2\2@>\3\2\2\2@A\3\2\2\2AC\3\2\2\2B@\3\2\2"+
		"\2CD\7\13\2\2DF\3\2\2\2E\64\3\2\2\2E;\3\2\2\2F\r\3\2\2\2GH\7\4\2\2HJ\7"+
		"\16\2\2IK\5\24\13\2JI\3\2\2\2JK\3\2\2\2KY\3\2\2\2LM\7\7\2\2MO\7\16\2\2"+
		"NP\5\24\13\2ON\3\2\2\2OP\3\2\2\2PY\3\2\2\2QR\7\5\2\2RT\7\16\2\2SU\5\24"+
		"\13\2TS\3\2\2\2TU\3\2\2\2UY\3\2\2\2VY\7\6\2\2WY\7\b\2\2XG\3\2\2\2XL\3"+
		"\2\2\2XQ\3\2\2\2XV\3\2\2\2XW\3\2\2\2Y\17\3\2\2\2Z[\7\16\2\2[\\\7\17\2"+
		"\2\\]\7\16\2\2]\21\3\2\2\2^_\7\16\2\2_\23\3\2\2\2`a\t\2\2\2a\25\3\2\2"+
		"\2\16\27\33+-\628@EJOTX";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}
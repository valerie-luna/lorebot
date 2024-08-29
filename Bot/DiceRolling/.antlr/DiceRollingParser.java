// Generated from /home/redacted/ShadowrunWeatherBot/DiceRolling/DiceRollingParser.g4 by ANTLR 4.9.2
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class DiceRollingParser extends Parser {
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
		RULE_request = 0, RULE_diceRoll = 1, RULE_reason = 2, RULE_multiplier = 3, 
		RULE_arithmetic = 4, RULE_expr = 5, RULE_diceModifiers = 6, RULE_diceAffix = 7, 
		RULE_sugarDice = 8, RULE_dice = 9, RULE_integer = 10, RULE_sign = 11;
	private static String[] makeRuleNames() {
		return new String[] {
			"request", "diceRoll", "reason", "multiplier", "arithmetic", "expr", 
			"diceModifiers", "diceAffix", "sugarDice", "dice", "integer", "sign"
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

	@Override
	public String getGrammarFileName() { return "DiceRollingParser.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public DiceRollingParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	public static class RequestContext extends ParserRuleContext {
		public List<DiceRollContext> diceRoll() {
			return getRuleContexts(DiceRollContext.class);
		}
		public DiceRollContext diceRoll(int i) {
			return getRuleContext(DiceRollContext.class,i);
		}
		public ReasonContext reason() {
			return getRuleContext(ReasonContext.class,0);
		}
		public RequestContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_request; }
	}

	public final RequestContext request() throws RecognitionException {
		RequestContext _localctx = new RequestContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_request);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(25); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(24);
				diceRoll();
				}
				}
				setState(27); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << SS_SHADOWRUN) | (1L << SS_OLD_SHADOWRUN) | (1L << SS_EARTHDAWN) | (1L << KW_TARGET) | (1L << OPENBRACKET) | (1L << SPACEDINT) | (1L << INT) | (1L << DICE))) != 0) );
			setState(30);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMENT) {
				{
				setState(29);
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

	public static class DiceRollContext extends ParserRuleContext {
		public ArithmeticContext arithmetic() {
			return getRuleContext(ArithmeticContext.class,0);
		}
		public MultiplierContext multiplier() {
			return getRuleContext(MultiplierContext.class,0);
		}
		public DiceRollContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_diceRoll; }
	}

	public final DiceRollContext diceRoll() throws RecognitionException {
		DiceRollContext _localctx = new DiceRollContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_diceRoll);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(33);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,2,_ctx) ) {
			case 1:
				{
				setState(32);
				multiplier();
				}
				break;
			}
			setState(35);
			arithmetic(0);
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
		public TerminalNode COMMENT() { return getToken(DiceRollingParser.COMMENT, 0); }
		public TerminalNode ANYTHING() { return getToken(DiceRollingParser.ANYTHING, 0); }
		public ReasonContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_reason; }
	}

	public final ReasonContext reason() throws RecognitionException {
		ReasonContext _localctx = new ReasonContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_reason);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(37);
			match(COMMENT);
			setState(38);
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
		public TerminalNode SPACEDINT() { return getToken(DiceRollingParser.SPACEDINT, 0); }
		public MultiplierContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_multiplier; }
	}

	public final MultiplierContext multiplier() throws RecognitionException {
		MultiplierContext _localctx = new MultiplierContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_multiplier);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(40);
			match(SPACEDINT);
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
		public TerminalNode OPENBRACKET() { return getToken(DiceRollingParser.OPENBRACKET, 0); }
		public List<ArithmeticContext> arithmetic() {
			return getRuleContexts(ArithmeticContext.class);
		}
		public ArithmeticContext arithmetic(int i) {
			return getRuleContext(ArithmeticContext.class,i);
		}
		public TerminalNode CLOSEBRACKET() { return getToken(DiceRollingParser.CLOSEBRACKET, 0); }
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public TerminalNode MULTIPLY() { return getToken(DiceRollingParser.MULTIPLY, 0); }
		public TerminalNode DIVIDE() { return getToken(DiceRollingParser.DIVIDE, 0); }
		public TerminalNode PLUS() { return getToken(DiceRollingParser.PLUS, 0); }
		public TerminalNode MINUS() { return getToken(DiceRollingParser.MINUS, 0); }
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
		int _startState = 8;
		enterRecursionRule(_localctx, 8, RULE_arithmetic, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(48);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case OPENBRACKET:
				{
				setState(43);
				match(OPENBRACKET);
				setState(44);
				arithmetic(0);
				setState(45);
				match(CLOSEBRACKET);
				}
				break;
			case SS_SHADOWRUN:
			case SS_OLD_SHADOWRUN:
			case SS_EARTHDAWN:
			case KW_TARGET:
			case SPACEDINT:
			case INT:
			case DICE:
				{
				setState(47);
				expr();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			_ctx.stop = _input.LT(-1);
			setState(64);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,5,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(62);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,4,_ctx) ) {
					case 1:
						{
						_localctx = new ArithmeticContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_arithmetic);
						setState(50);
						if (!(precpred(_ctx, 4))) throw new FailedPredicateException(this, "precpred(_ctx, 4)");
						setState(51);
						match(MULTIPLY);
						setState(52);
						arithmetic(5);
						}
						break;
					case 2:
						{
						_localctx = new ArithmeticContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_arithmetic);
						setState(53);
						if (!(precpred(_ctx, 3))) throw new FailedPredicateException(this, "precpred(_ctx, 3)");
						setState(54);
						match(DIVIDE);
						setState(55);
						arithmetic(4);
						}
						break;
					case 3:
						{
						_localctx = new ArithmeticContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_arithmetic);
						setState(56);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(57);
						match(PLUS);
						setState(58);
						arithmetic(3);
						}
						break;
					case 4:
						{
						_localctx = new ArithmeticContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_arithmetic);
						setState(59);
						if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
						setState(60);
						match(MINUS);
						setState(61);
						arithmetic(2);
						}
						break;
					}
					} 
				}
				setState(66);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,5,_ctx);
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
		enterRule(_localctx, 10, RULE_expr);
		try {
			setState(69);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,6,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(67);
				dice();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(68);
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
		public TerminalNode KW_EXPLOSION() { return getToken(DiceRollingParser.KW_EXPLOSION, 0); }
		public IntegerContext integer() {
			return getRuleContext(IntegerContext.class,0);
		}
		public SignContext sign() {
			return getRuleContext(SignContext.class,0);
		}
		public DMExplosionContext(DiceModifiersContext ctx) { copyFrom(ctx); }
	}
	public static class DMTargetContext extends DiceModifiersContext {
		public TerminalNode KW_TARGET() { return getToken(DiceRollingParser.KW_TARGET, 0); }
		public IntegerContext integer() {
			return getRuleContext(IntegerContext.class,0);
		}
		public SignContext sign() {
			return getRuleContext(SignContext.class,0);
		}
		public DMTargetContext(DiceModifiersContext ctx) { copyFrom(ctx); }
	}
	public static class DMIndefiniteContext extends DiceModifiersContext {
		public TerminalNode KW_INDEFINITE_EXPLOSION() { return getToken(DiceRollingParser.KW_INDEFINITE_EXPLOSION, 0); }
		public IntegerContext integer() {
			return getRuleContext(IntegerContext.class,0);
		}
		public SignContext sign() {
			return getRuleContext(SignContext.class,0);
		}
		public DMIndefiniteContext(DiceModifiersContext ctx) { copyFrom(ctx); }
	}
	public static class DMUnsortContext extends DiceModifiersContext {
		public TerminalNode KW_UNSORT() { return getToken(DiceRollingParser.KW_UNSORT, 0); }
		public DMUnsortContext(DiceModifiersContext ctx) { copyFrom(ctx); }
	}

	public final DiceModifiersContext diceModifiers() throws RecognitionException {
		DiceModifiersContext _localctx = new DiceModifiersContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_diceModifiers);
		try {
			setState(87);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case KW_TARGET:
				_localctx = new DMTargetContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(71);
				match(KW_TARGET);
				setState(72);
				integer();
				setState(74);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,7,_ctx) ) {
				case 1:
					{
					setState(73);
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
				setState(76);
				match(KW_EXPLOSION);
				setState(77);
				integer();
				setState(79);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,8,_ctx) ) {
				case 1:
					{
					setState(78);
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
				setState(81);
				match(KW_INDEFINITE_EXPLOSION);
				setState(82);
				integer();
				setState(84);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,9,_ctx) ) {
				case 1:
					{
					setState(83);
					sign();
					}
					break;
				}
				}
				break;
			case KW_UNSORT:
				_localctx = new DMUnsortContext(_localctx);
				enterOuterAlt(_localctx, 4);
				{
				setState(86);
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

	public static class DiceAffixContext extends ParserRuleContext {
		public DiceAffixContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_diceAffix; }
	 
		public DiceAffixContext() { }
		public void copyFrom(DiceAffixContext ctx) {
			super.copyFrom(ctx);
		}
	}
	public static class DPTotallingContext extends DiceAffixContext {
		public TerminalNode KW_TARGET() { return getToken(DiceRollingParser.KW_TARGET, 0); }
		public DPTotallingContext(DiceAffixContext ctx) { copyFrom(ctx); }
	}

	public final DiceAffixContext diceAffix() throws RecognitionException {
		DiceAffixContext _localctx = new DiceAffixContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_diceAffix);
		try {
			_localctx = new DPTotallingContext(_localctx);
			enterOuterAlt(_localctx, 1);
			{
			setState(89);
			match(KW_TARGET);
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

	public static class SugarDiceContext extends ParserRuleContext {
		public SugarDiceContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_sugarDice; }
	 
		public SugarDiceContext() { }
		public void copyFrom(SugarDiceContext ctx) {
			super.copyFrom(ctx);
		}
	}
	public static class SSOldShadowrunContext extends SugarDiceContext {
		public TerminalNode SS_OLD_SHADOWRUN() { return getToken(DiceRollingParser.SS_OLD_SHADOWRUN, 0); }
		public List<IntegerContext> integer() {
			return getRuleContexts(IntegerContext.class);
		}
		public IntegerContext integer(int i) {
			return getRuleContext(IntegerContext.class,i);
		}
		public TerminalNode KW_TARGET() { return getToken(DiceRollingParser.KW_TARGET, 0); }
		public SignContext sign() {
			return getRuleContext(SignContext.class,0);
		}
		public SSOldShadowrunContext(SugarDiceContext ctx) { copyFrom(ctx); }
	}
	public static class SSEarthdawnContext extends SugarDiceContext {
		public TerminalNode SS_EARTHDAWN() { return getToken(DiceRollingParser.SS_EARTHDAWN, 0); }
		public IntegerContext integer() {
			return getRuleContext(IntegerContext.class,0);
		}
		public SSEarthdawnContext(SugarDiceContext ctx) { copyFrom(ctx); }
	}
	public static class SSShadowrunContext extends SugarDiceContext {
		public TerminalNode SS_SHADOWRUN() { return getToken(DiceRollingParser.SS_SHADOWRUN, 0); }
		public List<IntegerContext> integer() {
			return getRuleContexts(IntegerContext.class);
		}
		public IntegerContext integer(int i) {
			return getRuleContext(IntegerContext.class,i);
		}
		public TerminalNode KW_INDEFINITE_EXPLOSION() { return getToken(DiceRollingParser.KW_INDEFINITE_EXPLOSION, 0); }
		public SignContext sign() {
			return getRuleContext(SignContext.class,0);
		}
		public SSShadowrunContext(SugarDiceContext ctx) { copyFrom(ctx); }
	}

	public final SugarDiceContext sugarDice() throws RecognitionException {
		SugarDiceContext _localctx = new SugarDiceContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_sugarDice);
		try {
			setState(111);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case SS_SHADOWRUN:
				_localctx = new SSShadowrunContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(91);
				match(SS_SHADOWRUN);
				setState(92);
				integer();
				setState(98);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,12,_ctx) ) {
				case 1:
					{
					setState(93);
					match(KW_INDEFINITE_EXPLOSION);
					setState(94);
					integer();
					setState(96);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,11,_ctx) ) {
					case 1:
						{
						setState(95);
						sign();
						}
						break;
					}
					}
					break;
				}
				}
				break;
			case SS_OLD_SHADOWRUN:
				_localctx = new SSOldShadowrunContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(100);
				match(SS_OLD_SHADOWRUN);
				setState(101);
				integer();
				setState(107);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,14,_ctx) ) {
				case 1:
					{
					setState(102);
					match(KW_TARGET);
					setState(103);
					integer();
					setState(105);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,13,_ctx) ) {
					case 1:
						{
						setState(104);
						sign();
						}
						break;
					}
					}
					break;
				}
				}
				break;
			case SS_EARTHDAWN:
				_localctx = new SSEarthdawnContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(109);
				match(SS_EARTHDAWN);
				setState(110);
				integer();
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

	public static class DiceContext extends ParserRuleContext {
		public DiceContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_dice; }
	 
		public DiceContext() { }
		public void copyFrom(DiceContext ctx) {
			super.copyFrom(ctx);
		}
	}
	public static class DiceRawContext extends DiceContext {
		public TerminalNode DICE() { return getToken(DiceRollingParser.DICE, 0); }
		public IntegerContext integer() {
			return getRuleContext(IntegerContext.class,0);
		}
		public TerminalNode INT() { return getToken(DiceRollingParser.INT, 0); }
		public List<DiceAffixContext> diceAffix() {
			return getRuleContexts(DiceAffixContext.class);
		}
		public DiceAffixContext diceAffix(int i) {
			return getRuleContext(DiceAffixContext.class,i);
		}
		public List<DiceModifiersContext> diceModifiers() {
			return getRuleContexts(DiceModifiersContext.class);
		}
		public DiceModifiersContext diceModifiers(int i) {
			return getRuleContext(DiceModifiersContext.class,i);
		}
		public DiceRawContext(DiceContext ctx) { copyFrom(ctx); }
	}
	public static class DiceSugarContext extends DiceContext {
		public SugarDiceContext sugarDice() {
			return getRuleContext(SugarDiceContext.class,0);
		}
		public DiceSugarContext(DiceContext ctx) { copyFrom(ctx); }
	}

	public final DiceContext dice() throws RecognitionException {
		DiceContext _localctx = new DiceContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_dice);
		int _la;
		try {
			int _alt;
			setState(137);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case SS_SHADOWRUN:
			case SS_OLD_SHADOWRUN:
			case SS_EARTHDAWN:
				_localctx = new DiceSugarContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(113);
				sugarDice();
				}
				break;
			case KW_TARGET:
			case INT:
			case DICE:
				_localctx = new DiceRawContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(115);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==INT) {
					{
					setState(114);
					match(INT);
					}
				}

				setState(120);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==KW_TARGET) {
					{
					{
					setState(117);
					diceAffix();
					}
					}
					setState(122);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(123);
				match(DICE);
				setState(127);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==KW_TARGET) {
					{
					{
					setState(124);
					diceAffix();
					}
					}
					setState(129);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(130);
				integer();
				setState(134);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,19,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(131);
						diceModifiers();
						}
						} 
					}
					setState(136);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,19,_ctx);
				}
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

	public static class IntegerContext extends ParserRuleContext {
		public TerminalNode INT() { return getToken(DiceRollingParser.INT, 0); }
		public TerminalNode SPACEDINT() { return getToken(DiceRollingParser.SPACEDINT, 0); }
		public IntegerContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_integer; }
	}

	public final IntegerContext integer() throws RecognitionException {
		IntegerContext _localctx = new IntegerContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_integer);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(139);
			_la = _input.LA(1);
			if ( !(_la==SPACEDINT || _la==INT) ) {
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

	public static class SignContext extends ParserRuleContext {
		public TerminalNode PLUS() { return getToken(DiceRollingParser.PLUS, 0); }
		public TerminalNode MINUS() { return getToken(DiceRollingParser.MINUS, 0); }
		public SignContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_sign; }
	}

	public final SignContext sign() throws RecognitionException {
		SignContext _localctx = new SignContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_sign);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(141);
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
		case 4:
			return arithmetic_sempred((ArithmeticContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean arithmetic_sempred(ArithmeticContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 4);
		case 1:
			return precpred(_ctx, 3);
		case 2:
			return precpred(_ctx, 2);
		case 3:
			return precpred(_ctx, 1);
		}
		return true;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3\25\u0092\4\2\t\2"+
		"\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13"+
		"\t\13\4\f\t\f\4\r\t\r\3\2\6\2\34\n\2\r\2\16\2\35\3\2\5\2!\n\2\3\3\5\3"+
		"$\n\3\3\3\3\3\3\4\3\4\3\4\3\5\3\5\3\6\3\6\3\6\3\6\3\6\3\6\5\6\63\n\6\3"+
		"\6\3\6\3\6\3\6\3\6\3\6\3\6\3\6\3\6\3\6\3\6\3\6\7\6A\n\6\f\6\16\6D\13\6"+
		"\3\7\3\7\5\7H\n\7\3\b\3\b\3\b\5\bM\n\b\3\b\3\b\3\b\5\bR\n\b\3\b\3\b\3"+
		"\b\5\bW\n\b\3\b\5\bZ\n\b\3\t\3\t\3\n\3\n\3\n\3\n\3\n\5\nc\n\n\5\ne\n\n"+
		"\3\n\3\n\3\n\3\n\3\n\5\nl\n\n\5\nn\n\n\3\n\3\n\5\nr\n\n\3\13\3\13\5\13"+
		"v\n\13\3\13\7\13y\n\13\f\13\16\13|\13\13\3\13\3\13\7\13\u0080\n\13\f\13"+
		"\16\13\u0083\13\13\3\13\3\13\7\13\u0087\n\13\f\13\16\13\u008a\13\13\5"+
		"\13\u008c\n\13\3\f\3\f\3\r\3\r\3\r\2\3\n\16\2\4\6\b\n\f\16\20\22\24\26"+
		"\30\2\4\3\2\22\23\3\2\16\17\2\u009f\2\33\3\2\2\2\4#\3\2\2\2\6\'\3\2\2"+
		"\2\b*\3\2\2\2\n\62\3\2\2\2\fG\3\2\2\2\16Y\3\2\2\2\20[\3\2\2\2\22q\3\2"+
		"\2\2\24\u008b\3\2\2\2\26\u008d\3\2\2\2\30\u008f\3\2\2\2\32\34\5\4\3\2"+
		"\33\32\3\2\2\2\34\35\3\2\2\2\35\33\3\2\2\2\35\36\3\2\2\2\36 \3\2\2\2\37"+
		"!\5\6\4\2 \37\3\2\2\2 !\3\2\2\2!\3\3\2\2\2\"$\5\b\5\2#\"\3\2\2\2#$\3\2"+
		"\2\2$%\3\2\2\2%&\5\n\6\2&\5\3\2\2\2\'(\7\13\2\2()\7\25\2\2)\7\3\2\2\2"+
		"*+\7\22\2\2+\t\3\2\2\2,-\b\6\1\2-.\7\f\2\2./\5\n\6\2/\60\7\r\2\2\60\63"+
		"\3\2\2\2\61\63\5\f\7\2\62,\3\2\2\2\62\61\3\2\2\2\63B\3\2\2\2\64\65\f\6"+
		"\2\2\65\66\7\20\2\2\66A\5\n\6\7\678\f\5\2\289\7\21\2\29A\5\n\6\6:;\f\4"+
		"\2\2;<\7\16\2\2<A\5\n\6\5=>\f\3\2\2>?\7\17\2\2?A\5\n\6\4@\64\3\2\2\2@"+
		"\67\3\2\2\2@:\3\2\2\2@=\3\2\2\2AD\3\2\2\2B@\3\2\2\2BC\3\2\2\2C\13\3\2"+
		"\2\2DB\3\2\2\2EH\5\24\13\2FH\5\26\f\2GE\3\2\2\2GF\3\2\2\2H\r\3\2\2\2I"+
		"J\7\7\2\2JL\5\26\f\2KM\5\30\r\2LK\3\2\2\2LM\3\2\2\2MZ\3\2\2\2NO\7\t\2"+
		"\2OQ\5\26\f\2PR\5\30\r\2QP\3\2\2\2QR\3\2\2\2RZ\3\2\2\2ST\7\b\2\2TV\5\26"+
		"\f\2UW\5\30\r\2VU\3\2\2\2VW\3\2\2\2WZ\3\2\2\2XZ\7\n\2\2YI\3\2\2\2YN\3"+
		"\2\2\2YS\3\2\2\2YX\3\2\2\2Z\17\3\2\2\2[\\\7\7\2\2\\\21\3\2\2\2]^\7\4\2"+
		"\2^d\5\26\f\2_`\7\b\2\2`b\5\26\f\2ac\5\30\r\2ba\3\2\2\2bc\3\2\2\2ce\3"+
		"\2\2\2d_\3\2\2\2de\3\2\2\2er\3\2\2\2fg\7\5\2\2gm\5\26\f\2hi\7\7\2\2ik"+
		"\5\26\f\2jl\5\30\r\2kj\3\2\2\2kl\3\2\2\2ln\3\2\2\2mh\3\2\2\2mn\3\2\2\2"+
		"nr\3\2\2\2op\7\6\2\2pr\5\26\f\2q]\3\2\2\2qf\3\2\2\2qo\3\2\2\2r\23\3\2"+
		"\2\2s\u008c\5\22\n\2tv\7\23\2\2ut\3\2\2\2uv\3\2\2\2vz\3\2\2\2wy\5\20\t"+
		"\2xw\3\2\2\2y|\3\2\2\2zx\3\2\2\2z{\3\2\2\2{}\3\2\2\2|z\3\2\2\2}\u0081"+
		"\7\24\2\2~\u0080\5\20\t\2\177~\3\2\2\2\u0080\u0083\3\2\2\2\u0081\177\3"+
		"\2\2\2\u0081\u0082\3\2\2\2\u0082\u0084\3\2\2\2\u0083\u0081\3\2\2\2\u0084"+
		"\u0088\5\26\f\2\u0085\u0087\5\16\b\2\u0086\u0085\3\2\2\2\u0087\u008a\3"+
		"\2\2\2\u0088\u0086\3\2\2\2\u0088\u0089\3\2\2\2\u0089\u008c\3\2\2\2\u008a"+
		"\u0088\3\2\2\2\u008bs\3\2\2\2\u008bu\3\2\2\2\u008c\25\3\2\2\2\u008d\u008e"+
		"\t\2\2\2\u008e\27\3\2\2\2\u008f\u0090\t\3\2\2\u0090\31\3\2\2\2\27\35 "+
		"#\62@BGLQVYbdkmquz\u0081\u0088\u008b";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}
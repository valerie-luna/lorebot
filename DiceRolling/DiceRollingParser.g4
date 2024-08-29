parser grammar DiceRollingParser;

options { tokenVocab=DiceRollingLexer; }

request: diceRoll+ reason?;

reason: COMMENT ANYTHING*;

diceRoll: multiplier? dicerollType;

multiplier: POSITIVE_INTEGER COLON ;

dicerollType: arithmetic | nonarithmetic ;

arithmetic: OPENBRACKET arithmetic CLOSEBRACKET
    | value
    | arithmetic MULTIPLY arithmetic
    | arithmetic DIVIDE arithmetic
    | arithmetic PLUS arithmetic
    | arithmetic MINUS arithmetic
    | arithmetic MODULO arithmetic
    ;

nonarithmetic: SS_GENESYS (genesysDice | genesysLiteral)+ #SSGenesys
    ;

value: dice | sign? (DECIMAL | POSITIVE_INTEGER);

dice: sugarDice #DiceSugar
    | diceNumber? singleDiceRoll groupKeep? groupTarget? groupLimit? #DiceGroup
    ;

groupTarget: KW_TARGET diceNumber sign?;
groupKeep: KW_KEEP diceNumber sign;
groupLimit: KW_LIMIT diceNumber;

singleDiceRoll: DICE diceNumber diceModifiers* ;

diceNumber: POSITIVE_INTEGER | squaredValue;
squaredValue: SQUARE_OPEN arithmetic SQUARE_CLOSED;

diceModifiers: KW_EXPLOSION diceNumber sign? #DMExplosion
    | KW_INDEFINITE KW_EXPLOSION diceNumber sign? #DMIndefiniteExplosion
    | KW_STACKING_EXPLOSION KW_EXPLOSION diceNumber sign? #DMStackingExplosion
    ;

sugarDice: SS_SHADOWRUN diceNumber shadowrunExplode? shadowrunLimit? (KW_VS value)? #SSShadowrun
    | SS_OLD_SHADOWRUN diceNumber oldShadowrunTarget? #SSOldShadowrun
    | SS_EARTHDAWN POSITIVE_INTEGER (KW_VS value)? #SSEarthdawn
    | SS_STARWARS POSITIVE_INTEGER DICE? #SSStarwars
    ;

shadowrunExplode: KW_INDEFINITE KW_EXPLOSION diceNumber? sign?;
shadowrunLimit: KW_LIMIT diceNumber;
oldShadowrunTarget: KW_TARGET diceNumber;

genesysDice: YELLOW | GREEN | BLUE | BLACK | RED | PURPLE ;
genesysLiteral: SUCCESS | FAILURE | ADVANTAGE | THREAT | TRIUMPH | DESPAIR ;

sign: PLUS | MINUS;
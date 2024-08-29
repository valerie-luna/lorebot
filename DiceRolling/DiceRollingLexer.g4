lexer grammar DiceRollingLexer;
options { caseInsensitive = true; }

WHITESPACE: ' '+ -> skip ;
fragment POSDIGIT: [1-9] ;
fragment DIGIT: [0-9] ;
POINT: '.' ;

SS_SHADOWRUN: 'sr' ;
SS_OLD_SHADOWRUN: 'osr' ;
SS_EARTHDAWN: 'ed' ;
SS_GENESYS: 'g:' -> pushMode(GENESYS);
SS_STARWARS: 'sw';


KW_TARGET: 't' ;
KW_INDEFINITE: 'i' ;
KW_STACKING_EXPLOSION: 's';
KW_EXPLOSION: 'e' ;
KW_UNSORT: 'unsort' ;
KW_KEEP: 'k' ;
KW_LIMIT: 'l';
KW_VS: 'vs' | 'vs.' ;


COMMENT: '!' -> mode(COMMENTARY);
OPENBRACKET: '(' ;
CLOSEBRACKET: ')' ;
SQUARE_OPEN: '[';
SQUARE_CLOSED: ']';
PLUS: '+' ;
MINUS: '-' ;
MULTIPLY: '*';
DIVIDE: '/';
MODULO: '%';
COLON: ':';
POSITIVE_INTEGER: POSDIGIT DIGIT* ;
DECIMAL: DIGIT+ | DIGIT+ '.' DIGIT+;
DICE: 'd';

ERROR: .;
mode COMMENTARY;

ANYTHING: . ;

mode GENESYS;

WHITESPACE_GENESYS: ' '+ -> skip ;

YELLOW: 'y';
GREEN: 'g';
BLUE: 'b';
BLACK: 'k';
RED: 'r';
PURPLE: 'p';

SUCCESS: 's';
FAILURE: 'f';
ADVANTAGE: 'a';
THREAT: 't';
TRIUMPH: 'i';
DESPAIR: 'd';
COMMENT_GENESYS: '!' -> mode(COMMENTARY);

ERROR_GENESYS: .;
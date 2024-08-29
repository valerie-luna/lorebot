lexer grammar MuxLexer;

WHITESPACE: ' '+ -> skip;

TRACK: 'track';
SIGNAL: 'signal';
EXPORT: 'export';

SEMICOLON: ';';
COLON: ':';
COMMA: ',';
BRACKET_OPEN: '(';
BRACKET_CLOSE: ')';

IDENTIFIER: .+? ;
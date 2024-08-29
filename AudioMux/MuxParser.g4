parser grammar MuxParser;

options { tokenVocab=MuxLexer; }

statement: (track | signal | export) SEMICOLON;

track: TRACK IDENTIFIER expression;
signal: SIGNAL IDENTIFIER;
export: EXPORT IDENTIFIER;

expression: IDENTIFIER | func;

func: IDENTIFIER BRACKET_OPEN arguments sigSpec? BRACKET_CLOSE;
arguments: expression COMMA arguments | expression ;
sigSpec: COLON IDENTIFIER ;
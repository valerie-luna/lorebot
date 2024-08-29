namespace AudioMux.Libraries.Flac;

public enum SubframeType : int 
{
	Constant = 0, /**< constant signal */
	Verbatim = 1, /**< uncompressed signal */
	Fixed = 2, /**< fixed polynomial prediction */
	LPC = 3 /**< linear prediction */
}

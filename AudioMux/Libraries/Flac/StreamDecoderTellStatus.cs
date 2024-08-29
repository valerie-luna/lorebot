namespace AudioMux.Libraries.Flac;

public enum StreamDecoderTellStatus : int
{
	Ok,
	/**< The tell was OK and decoding can continue. */

	Error,
	/**< An unrecoverable error occurred.  The decoder will return from the process call. */

	Unsupported
	/**< Client does not support telling the position. */
}

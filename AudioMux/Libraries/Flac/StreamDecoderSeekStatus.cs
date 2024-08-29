namespace AudioMux.Libraries.Flac;

public enum StreamDecoderSeekStatus : int
{
	Ok,
	/**< The seek was OK and decoding can continue. */

	Error,
	/**< An unrecoverable error occurred.  The decoder will return from the process call. */

	Unsupported
	/**< Client does not support seeking. */	
}

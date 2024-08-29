namespace AudioMux.Libraries.Flac;

public enum StreamDecoderLengthStatus : int
{
	Ok,
	/**< The length call was OK and decoding can continue. */

	Error,
	/**< An unrecoverable error occurred.  The decoder will return from the process call. */

	Unsupported
	/**< Client does not support reporting the length. */
}
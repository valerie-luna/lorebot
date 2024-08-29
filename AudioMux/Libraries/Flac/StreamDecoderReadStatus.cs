namespace AudioMux.Libraries.Flac;

public enum StreamDecoderReadStatus : int
{
	Continue,
	/**< The read was OK and decoding can continue. */

	EndOfStream,
	/**< The read was attempted while at the end of the stream.  Note that
	 * the client must only return this value when the read callback was
	 * called when already at the end of the stream.  Otherwise, if the read
	 * itself moves to the end of the stream, the client should still return
	 * the data and \c FLAC__STREAM_DECODER_READ_STATUS_CONTINUE, and then on
	 * the next read callback it should return
	 * \c FLAC__STREAM_DECODER_READ_STATUS_END_OF_STREAM with a byte count
	 * of \c 0.
	 */

	Abort
	/**< An unrecoverable error occurred.  The decoder will return from the process call. */
}

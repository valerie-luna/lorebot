namespace AudioMux.Libraries.Flac;

public enum StreamDecoderErrorStatus : int 
{

	LostSync,
	/**< An error in the stream caused the decoder to lose synchronization. */

	BadHeader,
	/**< The decoder encountered a corrupted frame header. */

	FrameCrcMismatch,
	/**< The frame's data did not match the CRC in the footer. */

	UnparseableStream,
	/**< The decoder encountered reserved fields in use in the stream. */

	BadMetadata
	/**< The decoder encountered a corrupted metadata block. */
}

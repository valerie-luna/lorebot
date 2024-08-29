namespace AudioMux.Libraries.Flac;

public enum StreamDecoderInitStatus : int 
{
	Ok = 0,
	UnsupportedContainer,
	InvalidCallbacks,
	MemoryAllocationError,
	ErrorOpeningFile,
	AlreadyInitialized
}

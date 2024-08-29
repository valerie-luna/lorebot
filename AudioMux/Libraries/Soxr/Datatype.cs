namespace AudioMux.Libraries.Soxr;

public enum Datatype 
{          /* Datatypes supported for I/O to/from the resampler: */
  /* Internal; do not use: */
  Float32, Float64, Int32, Int16, Split = 4,

  /* Use for interleaved channels: */
  Float32Interleaved = Float32, Float64Interleved, Int32Interleved, Int16Interleved,

  /* Use for split channels: */
  Float32Split = Split , Float64Split, Int32Split, Int16Split
}

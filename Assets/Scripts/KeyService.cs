namespace stinkeys {
  public enum KeyMode {
    UNDEFINED,
    NORMAL,
    FART,
  }

  public static class KeyService {
    public static KeyMode Mode { get; set; } = KeyMode.NORMAL;
  }
}
extension EmailValidatorExtension on String {
  static final RegExp _emailRegexp = RegExp(
      r'^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))'
      r'@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])'
      r'|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$');

  bool isValidEmail() {
    return _emailRegexp.hasMatch(this);
  }
}

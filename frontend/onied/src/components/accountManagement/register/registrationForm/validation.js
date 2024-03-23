export const validateInput = (field, input, setErrorMessage) => {
  let { name, value } = field;
  setErrorMessage((prev) => {
    const stateObj = { ...prev, [name]: "" };

    switch (name) {
      case "firstName":
        if (!value) {
          stateObj[name] = "Введите имя";
        }
        break;

      case "lastName":
        if (!value) {
          stateObj[name] = "Введите фамилию";
        }
        break;

      case "email":
        if (!value) {
          stateObj[name] = "Введите электронный адрес";
        }
        break;

      case "password":
        if (
          !/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*])[0-9a-zA-Z!@#$%^&*]{8,}$/.test(
            value
          )
        ) {
          stateObj[name] = "Введенный пароль слишком простой";
        } else if (input.confirmPassword && value !== input.confirmPassword) {
          stateObj["confirmPassword"] = "Введенные пароли не совпадают";
        } else {
          stateObj["confirmPassword"] = input.confirmPassword
            ? ""
            : "Введенные пароли не совпадают";
        }
        break;

      case "confirmPassword":
        if (input.password && value !== input.password) {
          stateObj[name] = "Введенные пароли не совпадают";
        }
        break;

      default:
        break;
    }

    return stateObj;
  });
};

export const handleErrors = (errors, setErrorMessage) => {
  const keys = Object.keys(errors);
  if (keys.includes("DuplicateUserName")) {
    setErrorMessage((prev) => ({
      ...prev,
      email: "Введенная электронная почта уже используется",
    }));
  } else if (keys.some((key) => key.startsWith("Password"))) {
    setErrorMessage((prev) => ({
      ...prev,
      password: "Введенный пароль слишком простой",
    }));
  }
};

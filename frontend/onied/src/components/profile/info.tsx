import classes from "./profile.module.css";
import { useProfile } from "../../hooks/profile/useProfile";
import InputForm from "../general/inputform/inputform";
import { ChangeEvent, useState } from "react";
import Radio from "../general/radio/radio";
import Button from "../general/button/button";
import Avatar from "react-avatar";
import { Profile, getProfileName } from "../../hooks/profile/profile";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";
import api from "../../config/axios";
import ProfileService from "../../services/profileService";
import LoginService from "../../services/loginService";
import { useNavigate } from "react-router-dom";

type Errors = {
  firstName: string | null;
  lastName: string | null;
  gender: string | null;
  avatar: string | null;
};

function ProfileInfo() {
  const navigate = useNavigate();
  const [originalProfile, _] = useProfile();
  const [profile, setProfile] = useState<Profile>({
    ...originalProfile,
  } as Profile);
  const [errors, setErrors] = useState<Errors>({
    firstName: null,
    lastName: null,
    gender: null,
    avatar: null,
  });
  const [newAvatar, setNewAvatar] = useState<string>("");
  const [avatarChangeModalOpen, setAvatarChangeModalOpen] = useState(false);
  const [passwordSent, setPasswordSent] = useState<Boolean>(false);
  const [profileInfoSaved, setProfileInfoSaved] = useState(false);
  const onGenderChange = (e: ChangeEvent<HTMLInputElement>) =>
    setProfile({ ...profile, gender: Number(e.target.value) });
  const saveInfo = (e: any) => {
    e.preventDefault();
    setProfileInfoSaved(false);
    let newErrors: Errors = {
      firstName: null,
      lastName: null,
      gender: null,
      avatar: null,
    };
    api
      .put("/profile", {
        firstName: profile.firstName,
        lastName: profile.lastName,
        gender: profile.gender,
      })
      .then((_) => {
        setProfileInfoSaved(true);
        ProfileService.fetchProfile();
      })
      .catch((error) => {
        if (error.response.status == 400) {
          if (error.response.data.errors.Gender)
            newErrors = { ...newErrors, gender: "Неверное значение поля" };
          if (error.response.data.errors.FirstName)
            newErrors = { ...newErrors, firstName: "Введите правильное имя" };
          if (error.response.data.errors.LastName)
            newErrors = {
              ...newErrors,
              lastName: "Введите правильную фамилию",
            };
        }
        setErrors(newErrors);
      });
  };
  const saveAvatar = (e: any) => {
    e.preventDefault();
    setErrors({
      firstName: null,
      lastName: null,
      gender: null,
      avatar: null,
    });
    api
      .put("/profile/avatar", {
        avatarHref: newAvatar,
      })
      .then((_) => {
        setAvatarChangeModalOpen(false);
        ProfileService.fetchProfile();
      })
      .catch((error) => {
        if (error.response.status == 400) {
          if (error.response.data.errors.AvatarHref) {
            setErrors({ ...errors, avatar: "Введите корректный URL." });
          }
        }
      });
  };
  const deleteAvatar = (e: any) => {
    e.preventDefault();
    setErrors({
      firstName: null,
      lastName: null,
      gender: null,
      avatar: null,
    });
    api
      .put("/profile/avatar", {
        avatarHref: null,
      })
      .then((_) => {
        setAvatarChangeModalOpen(false);
        ProfileService.fetchProfile();
      })
      .catch((error) => {
        console.log(error);
      });
  };
  const sendPasswordReset = () => {
    setPasswordSent(false);
    api
      .post("forgotPassword", {
        email: profile.email,
      })
      .then((_) => {
        setPasswordSent(true);
      })
      .catch();
  };
  const logout = () => {
    LoginService.unregisterAutomaticRefresh();
    LoginService.clearTokens();
    ProfileService.clearProfile();
    navigate("/");
  };
  if (originalProfile == null) return <></>;
  return (
    <div className={classes.pageWrapper}>
      <h3 className={classes.pageTitle}>Редактирование профиля</h3>
      <div className={classes.profileInfoContents}>
        <form onSubmit={saveInfo}>
          <div className={classes.profileInfoGrid}>
            <label
              htmlFor="profile_first_name"
              className={classes.profileInfoLabel}
            >
              Имя
            </label>
            <div className={classes.profileInfoInputWrapper}>
              <InputForm
                id="profile_first_name"
                value={profile.firstName}
                onChange={(e: ChangeEvent<HTMLInputElement>) =>
                  setProfile({ ...profile, firstName: e.target.value })
                }
              ></InputForm>
              {errors.firstName ? (
                <div className={classes.profileInfoError}>
                  {errors.firstName}
                </div>
              ) : (
                <></>
              )}
            </div>
            <label
              htmlFor="profile_last_name"
              className={classes.profileInfoLabel}
            >
              Фамилия
            </label>
            <div className={classes.profileInfoInputWrapper}>
              <InputForm
                id="profile_last_name"
                value={profile.lastName}
                onChange={(e: ChangeEvent<HTMLInputElement>) =>
                  setProfile({ ...profile, lastName: e.target.value })
                }
              ></InputForm>
              {errors.lastName ? (
                <div className={classes.profileInfoError}>
                  {errors.lastName}
                </div>
              ) : (
                <></>
              )}
            </div>
            <label
              htmlFor="profile_gender"
              className={classes.profileInfoLabel}
            >
              Пол
            </label>
            <div className={classes.profileInfoInputWrapper}>
              <div
                className={classes.profileInfoRadioGroup}
                id="profile_gender"
              >
                <div className={classes.profileInfoRadio}>
                  <label
                    htmlFor="profile_gender_other"
                    className={classes.profileInfoRadioLabel}
                  >
                    Не указан
                  </label>
                  <Radio
                    name="profile_gender"
                    id="profile_gender_other"
                    value="0"
                    checked={profile.gender == 0}
                    onChange={onGenderChange}
                  ></Radio>
                </div>
                <div className={classes.profileInfoRadio}>
                  <label
                    htmlFor="profile_gender_male"
                    className={classes.profileInfoRadioLabel}
                  >
                    Мужской
                  </label>
                  <Radio
                    name="profile_gender"
                    id="profile_gender_male"
                    value="1"
                    checked={profile.gender == 1}
                    onChange={onGenderChange}
                  ></Radio>
                </div>
                <div className={classes.profileInfoRadio}>
                  <label
                    htmlFor="profile_gender_female"
                    className={classes.profileInfoRadioLabel}
                  >
                    Женский
                  </label>
                  <Radio
                    name="profile_gender"
                    id="profile_gender_female"
                    value="2"
                    checked={profile.gender == 2}
                    onChange={onGenderChange}
                  ></Radio>
                </div>
              </div>

              {errors.gender ? (
                <div className={classes.profileInfoError}>{errors.gender}</div>
              ) : (
                <></>
              )}
            </div>
          </div>
          <div className={classes.profileInfoFooter}>
            <div className={classes.profileInfoRightButton}>
              <Button type="submit">сохранить</Button>
              {profileInfoSaved ? (
                <label className={classes.profileInfoSaved}>Сохранено</label>
              ) : (
                <></>
              )}
            </div>
          </div>
        </form>

        <hr className={classes.hr}></hr>

        <div className={classes.profileInfoGrid}>
          <label htmlFor="profile_avatar">Аватар</label>
          <div className={classes.profileInfoInputWrapper}>
            <Avatar
              name={getProfileName(profile)}
              size="150"
              className={classes.profileAvatar}
              src={originalProfile.avatar ? originalProfile.avatar : undefined}
            ></Avatar>
          </div>
        </div>
        <div className={classes.profileInfoFooter}>
          <div className={classes.profileInfoLeftButton}>
            <Button
              style={{ width: "100%" }}
              onClick={() => setAvatarChangeModalOpen(true)}
            >
              загрузить
            </Button>
          </div>
          <div className={classes.profileInfoRightButton}>
            <Button
              style={{ width: "100%" }}
              disabled={!originalProfile.avatar}
              onClick={deleteAvatar}
            >
              удалить
            </Button>
          </div>
        </div>

        <hr className={classes.hr}></hr>

        <div>
          <h3 className={classes.pageTitle}>Пароль</h3>
          <p className={classes.changePasswordDesc}>
            На вашу почту будет отправлена ссылка для смены пароля.
          </p>
          <Button disabled={passwordSent} onClick={sendPasswordReset}>
            сменить пароль
          </Button>
          {passwordSent ? (
            <p className={classes.changePasswordSuccess}>
              Письмо было отправлено на ваш электронный ящик.
            </p>
          ) : (
            <></>
          )}
        </div>

        <hr className={classes.hr}></hr>

        <Button onClick={logout}>Выйти</Button>
      </div>
      <Dialog
        open={avatarChangeModalOpen}
        onClose={() => setAvatarChangeModalOpen(false)}
        PaperProps={{
          component: "form",
          onSubmit: saveAvatar,
        }}
      >
        <DialogTitle>Загрузить аватар</DialogTitle>
        <DialogContent>
          <DialogContentText>Введите ссылку на новый аватар</DialogContentText>
          <InputForm
            value={newAvatar}
            onChange={(e: ChangeEvent<HTMLInputElement>) =>
              setNewAvatar(e.target.value)
            }
            style={{ margin: "1rem" }}
            placeholder="ссылка на новый аватар"
            type="url"
          ></InputForm>
          {errors.avatar ? (
            <div className={classes.profileInfoError}>{errors.avatar}</div>
          ) : (
            <></>
          )}
        </DialogContent>
        <DialogActions>
          <button type="submit" className={classes.avatarChangeDialogButton}>
            сохранить аватар
          </button>
        </DialogActions>
      </Dialog>
    </div>
  );
}

export default ProfileInfo;

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

type Errors = {
  firstName: string | null;
  lastName: string | null;
  gender: string | null;
  avatar: string | null;
};

function ProfileInfo() {
  const originalProfile = useProfile();
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
  const onGenderChange = (e: ChangeEvent<HTMLInputElement>) =>
    setProfile({ ...profile, gender: Number(e.target.value) });
  if (profile == null) return <></>;
  return (
    <div className={classes.pageWrapper}>
      <h3 className={classes.pageTitle}>Редактирование профиля</h3>
      <div className={classes.profileInfoContents}>
        <form>
          <div className={classes.profileInfoGrid}>
            <label
              htmlFor="profile_first_name"
              className={classes.profileInfoLabel}
            >
              Имя
            </label>
            <div className={classes.profileInfoInputWrapper}>
              <InputForm
                id="profile_last_name"
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
              src={profile.avatarHref ? profile.avatarHref : undefined}
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
            <Button style={{ width: "100%" }} disabled="t">
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
          <Button disabled={passwordSent}>сменить пароль</Button>
          {passwordSent ? (
            <p className={classes.changePasswordSuccess}>
              Письмо было отправлено на ваш электронный ящик.
            </p>
          ) : (
            <></>
          )}
        </div>
      </div>
      <Dialog
        open={avatarChangeModalOpen}
        onClose={() => setAvatarChangeModalOpen(false)}
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
            сохранить
          </button>
        </DialogActions>
      </Dialog>
    </div>
  );
}

export default ProfileInfo;

import { ChangeEvent, useEffect, useState } from "react";
import api from "../../../config/axios";
import { useParams, Link } from "react-router-dom";
import classes from "../editPreview/editPreview.module.css";
import imagePlaceholder from "../../../assets/imagePlaceholder.svg";
import Button from "../../general/button/button";
import InputForm from "../../general/inputform/inputform";
import InputFormArea from "../../general/inputform/inputFormArea";
import Select from "../../general/inputform/select";
import Checkbox from "../../general/checkbox/checkbox";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogActions from "@mui/material/DialogActions";
import PreviewModal from "./previewModal";
import BeatLoader from "react-spinners/BeatLoader";

type PreviewDto = {
  title: string;
  pictureHref: string;
  description: string;
  hoursCount: number;
  price: number;
  category: {
    id: number;
    name: string;
  };
  courseAuthor: {
    name: string;
    avatarHref: string;
  };
  isArchived: boolean;
  hasCertificates: boolean;
  isProgramVisible: boolean;
  courseProgram: Array<string> | undefined;
};

type CategoryDto = {
  id: number;
  name: string;
};

type Errors = {
  Title: string | null;
  Description: string | null;
  Price: string | null;
  CompleteTime: string | null;
  Picture: string | null;
};

function EditPreviewComponent() {
  const { courseId } = useParams();
  const [previewInfo, setPreview] = useState<PreviewDto | undefined>();
  const [categories, setCategories] = useState<
    Array<CategoryDto> | undefined
  >();
  const [errors, setErrors] = useState<Errors>({
    Title: null,
    Description: null,
    Price: null,
    CompleteTime: null,
    Picture: null,
  });
  const [found, setFound] = useState<boolean | undefined>();
  const [newImageHref, setNewImageHref] = useState<string>("");
  const [isNewImageModalOpen, setIsNewImageModalOpen] =
    useState<boolean>(false);
  const [isCheckPreviewModalOpen, setIsCheckPreviewModalOpen] =
    useState<boolean>(false);
  const [isNewPreviewInfoSaved, setIsNewPreviewInfoSaved] =
    useState<boolean>(false);
  const notFound = <h1 style={{ margin: "3rem" }}>Курс не найден.</h1>;

  const id = Number(courseId);
  if (isNaN(id)) return notFound;

  const saveChanges = () => {
    setIsNewPreviewInfoSaved(false);
    api
      .put("courses/" + courseId, {
        title: previewInfo?.title,
        description: previewInfo?.description,
        categoryId: previewInfo?.category.id,
        price: previewInfo?.price,
        hoursCount: previewInfo?.hoursCount,
        pictureHref: previewInfo?.pictureHref,
        isProgramVisible: previewInfo?.isProgramVisible,
        hasCertificates: previewInfo?.hasCertificates,
        isArchived: previewInfo?.isArchived,
      })
      .then((_) => {
        setIsNewPreviewInfoSaved(true);
      })
      .catch((error) => {
        if (error.response.status == 400) {
          if (error.response.data.errors.Title)
            setErrors({
              ...errors,
              Title:
                previewInfo?.title.length === 0
                  ? "Это обязательное поле"
                  : "Название не может иметь больше 200 символов",
            });
          if (error.response.data.errors.Description)
            setErrors({
              ...errors,
              Description:
                previewInfo?.description.length === 0
                  ? "Это обязательное поле"
                  : "Описание не может иметь больше 15000 символов",
            });
          if (
            error.response.data.errors.PictureHref &&
            previewInfo?.pictureHref.length !== 0
          )
            setErrors({ ...errors, Picture: "Введите корректный URL." });
          if (error.response.data.errors.HoursCount)
            setErrors({
              ...errors,
              CompleteTime: "Введите число от 0 до 35000",
            });
          if (error.response.data.errors.Price)
            setErrors({
              ...errors,
              Price: "Введите число от 0 до 1000000",
            });
        }
      });
  };

  const saveNewImage = (e: any) => {
    e.preventDefault();
    setPreview({ ...previewInfo!, pictureHref: newImageHref });
    setIsNewImageModalOpen(false);
  };

  useEffect(() => {
    setFound(undefined);
    api
      .get("courses/" + courseId)
      .then((response) => {
        console.log(response.data);
        setFound(true);
        setPreview(response.data);
        if (previewInfo?.pictureHref.trim().length === 0)
          setPreview({ ...previewInfo!, pictureHref: imagePlaceholder });
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          setFound(false);
        }
      });
  }, [courseId]);

  useEffect(() => {
    api
      .get("categories")
      .then((response) => {
        console.log(response.data);
        setCategories(response.data);
      })
      .catch((error) => {
        console.log(error);
      });
  }, []);

  if (previewInfo == undefined || categories == undefined)
    return <BeatLoader color="var(--accent-color)"></BeatLoader>;

  if (!found) return notFound;

  return (
    <>
      <div className={classes.firstBlock}>
        <form>
          <div className={classes.inputChanges}>
            <div className={classes.inputChangesOneColumn}>
              <label className={classes.formLabel} htmlFor="preview_title">
                Название
              </label>
              <div>
                <InputForm
                  id="preview_title"
                  value={previewInfo?.title}
                  onChange={(e: ChangeEvent<HTMLInputElement>) =>
                    setPreview({ ...previewInfo!, title: e.target.value })
                  }
                  style={{ position: "static" }}
                ></InputForm>
                {errors.Title ? (
                  <div className={classes.previewInfoError}>{errors.Title}</div>
                ) : (
                  <></>
                )}
              </div>
              <label
                className={classes.formLabel}
                htmlFor="preview_description"
              >
                Описание
              </label>
              <div className={classes.fixedHeightTextArea}>
                <InputFormArea
                  id="preview_description"
                  value={previewInfo?.description}
                  onChange={(e: ChangeEvent<HTMLInputElement>) =>
                    setPreview({ ...previewInfo!, description: e.target.value })
                  }
                ></InputFormArea>
                {errors.Description ? (
                  <div className={classes.previewInfoError}>
                    {errors.Description}
                  </div>
                ) : (
                  <></>
                )}
              </div>
              <label className={classes.formLabel} htmlFor="preview_category">
                Категория
              </label>
              <div className={classes.customSelect}>
                <Select
                  value={previewInfo.category.id}
                  onChange={(e: ChangeEvent<HTMLSelectElement>) => {
                    let categoryId = Number(e.target.value);
                    setPreview({
                      ...previewInfo!,
                      category: {
                        id: categoryId,
                        name: categories![categoryId - 1].name,
                      },
                    });
                  }}
                >
                  {categories!.map((categoryDto) => (
                    <option key={categoryDto.id} value={categoryDto.id}>
                      {categoryDto.name}
                    </option>
                  ))}
                </Select>
              </div>
            </div>
            <div className={classes.inputChangesTwoColumns}>
              <div className={classes.inputChangesBlock}>
                <label className={classes.formLabel} htmlFor="preview_price">
                  Цена
                </label>
                <label
                  className={classes.formFieldDescription}
                  htmlFor="preview_price"
                >
                  Введите 0, если хотите сделать курс бесплатным для всех
                </label>
                <div className={classes.fixedHeightTextArea}>
                  <InputForm
                    type="number"
                    id="preview_price"
                    value={previewInfo?.price}
                    onChange={(e: ChangeEvent<HTMLInputElement>) =>
                      setPreview({
                        ...previewInfo!,
                        price: Number(e.target.value),
                      })
                    }
                  ></InputForm>
                  {errors.Price ? (
                    <div className={classes.previewInfoError}>
                      {errors.Price}
                    </div>
                  ) : (
                    <></>
                  )}
                </div>
              </div>
              <div className={classes.inputChangesBlock}>
                <label
                  className={classes.formLabel}
                  htmlFor="preview_complete_time"
                >
                  Время прохождения
                </label>
                <label
                  className={classes.formFieldDescription}
                  htmlFor="preview_complete_time"
                >
                  Оценка времени, необходимого для прождения курса
                </label>
                <div className={classes.fixedHeightTextArea}>
                  <InputForm
                    type="number"
                    id="preview_complete_time"
                    value={previewInfo?.hoursCount}
                    onChange={(e: ChangeEvent<HTMLInputElement>) =>
                      setPreview({
                        ...previewInfo!,
                        hoursCount: Number(e.target.value),
                      })
                    }
                  ></InputForm>
                  {errors.CompleteTime ? (
                    <div className={classes.previewInfoError}>
                      {errors.CompleteTime}
                    </div>
                  ) : (
                    <></>
                  )}
                </div>
              </div>
            </div>
          </div>
        </form>
        <div className={classes.previewPrompt}>
          <span>
            Превью страница отображает основную информацию о вашем курсе
          </span>
          <Button
            style={{ width: "50%", margin: "25px 0" }}
            onClick={() => setIsCheckPreviewModalOpen(true)}
          >
            посмотреть
          </Button>
        </div>
        <div className={classes.newCoursePictureWrapper}>
          <div className={classes.newCoursePicture}>
            <label className={classes.formLabel}>Логотип</label>
            <img src={previewInfo.pictureHref} />
          </div>
          <Button
            style={{ width: "40%" }}
            onClick={() => setIsNewImageModalOpen(true)}
          >
            загрузить
          </Button>
        </div>
      </div>
      <div className={classes.line}></div>
      <div className={classes.toggleChanges}>
        <div className={classes.toggleChangeWrapper}>
          <div className={classes.checkboxAndTitle}>
            <Checkbox
              id="is-program-visible-checkbox"
              checked={previewInfo?.isProgramVisible}
              onChange={(e: ChangeEvent<HTMLInputElement>) =>
                setPreview({
                  ...previewInfo!,
                  isProgramVisible: e.target.checked,
                })
              }
            />
            <label
              className={classes.formLabel}
              htmlFor="is-program-visible-checkbox"
            >
              Содержание курса
            </label>
          </div>
          <label
            className={classes.formFieldDescription}
            htmlFor="is-program-visible-checkbox"
          >
            Содержание курса доступно на превью курса и показывает
            предварительную программу, составленную из списка модулей
          </label>
        </div>
        <div className={classes.toggleChangeWrapper}>
          <div className={classes.checkboxAndTitle}>
            <Checkbox
              id="has-certificates-checkbox"
              checked={previewInfo?.hasCertificates}
              onChange={(e: ChangeEvent<HTMLInputElement>) =>
                setPreview({
                  ...previewInfo!,
                  hasCertificates: e.target.checked,
                })
              }
            />
            <label
              className={classes.formLabel}
              htmlFor="has-certificates-checkbox"
            >
              Выдача сертификатов
            </label>
          </div>

          <label
            className={classes.formFieldDescription}
            htmlFor="has-certificates-checkbox"
          >
            Выдача сертификатов достпуна только с полной подпиской
          </label>
        </div>
        <div className={classes.toggleChangeWrapper}>
          <div className={classes.checkboxAndTitle}>
            <Checkbox
              id="is-archived-checkbox"
              checked={previewInfo?.isArchived}
              onChange={(e: ChangeEvent<HTMLInputElement>) =>
                setPreview({
                  ...previewInfo!,
                  isArchived: e.target.checked,
                })
              }
            />
            <label className={classes.formLabel} htmlFor="is-archived-checkbox">
              <p>Заархивировать курс</p>
            </label>
          </div>

          <label
            className={classes.formFieldDescription}
            htmlFor="is-archived-checkbox"
          >
            Курс может уйти в архив, тогда он будет недоступен для покупки
            (ученик может не платить за него, он считается бесплатным).
          </label>
        </div>
      </div>
      <div className={classes.line}></div>
      <div className={classes.footerButtons}>
        <div>
          <Button onClick={saveChanges}>сохранить изменения</Button>
          {isNewPreviewInfoSaved ? (
            <label className={classes.previewInfoSaved}>Сохранено</label>
          ) : (
            <></>
          )}
        </div>
        <Link to={`../course/${courseId}/edit/hierarchy`}>
          <Button
            style={{
              boxShadow: "none",
              backgroundColor: "white",
              border: "3px solid var(--button-color)",
              cursor: "inherit",
              color: "var(--accent-color)",
            }}
          >
            редактировать содержимое курса
          </Button>
        </Link>
      </div>
      <Dialog
        open={isNewImageModalOpen}
        onClose={() => setIsNewImageModalOpen(false)}
        PaperProps={{
          component: "form",
          onSubmit: saveNewImage,
        }}
      >
        <DialogTitle>Загрузить файл</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Введите сслыку на ваше изображение
          </DialogContentText>
          <InputForm
            value={newImageHref}
            onChange={(e: ChangeEvent<HTMLInputElement>) =>
              setNewImageHref(e.target.value)
            }
            style={{ margin: "1rem" }}
            type="url"
          ></InputForm>
        </DialogContent>
        <DialogActions>
          <button type="submit" className={classes.submitNewFileButton}>
            сохранить
          </button>
        </DialogActions>
      </Dialog>
      <Dialog
        fullWidth={true}
        maxWidth={"xl"}
        open={isCheckPreviewModalOpen}
        onClose={() => setIsCheckPreviewModalOpen(false)}
      >
        <DialogContent>
          <PreviewModal
            {...{ ...previewInfo!, onCloseClick: setIsCheckPreviewModalOpen }}
          />
        </DialogContent>
      </Dialog>
    </>
  );
}

export default EditPreviewComponent;

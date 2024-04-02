import React, { ChangeEvent, useEffect, useState } from "react";
import api from "../../../config/axios";
import { useNavigate, useParams, Link } from "react-router-dom";
import classes from "../editPreview/editPreview.module.css";
import Button from "../../general/button/button";
import InputForm from "../../general/inputform/inputform";
import InputFormArea from "../../general/inputform/inputFormArea";
import Checkbox from "../../general/checkbox/checkbox";

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
  isArchived: boolean;
  hasCertificates: boolean;
  isContentProgramVisible: boolean;
};

type Errors = {
  Title: string | null;
  Description: string | null;
  Category: string | null;
  Price: string | null;
  CompleteTime: string | null;
  Picture: string | null;
};

function EditPreviewComponent() {
  const navigator = useNavigate();
  const { courseId } = useParams();
  const [previewInfo, setPreview] = useState<PreviewDto | undefined>();
  const [errors, setErrors] = useState<Errors>({
    Title: null,
    Description: null,
    Category: null,
    Price: null,
    CompleteTime: null,
    Picture: null,
  });
  const [found, setFound] = useState<boolean | undefined>();
  const notFound = <h1 style={{ margin: "3rem" }}>Курс не найден.</h1>;

  const id = Number(courseId);
  if (isNaN(id)) return notFound;

  const checkPreview = () => {};

  useEffect(() => {
    setFound(undefined);
    api
      .get("courses/" + id)
      .then((response) => {
        console.log(response.data);
        setFound(true);
        setPreview(response.data);
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          setFound(false);
        }
      });
  }, [courseId]);

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
                {errors.Title ? <div>{errors.Title}</div> : <></>}
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
                {errors.Description ? <div>{errors.Description}</div> : <></>}
              </div>
              <label className={classes.formLabel} htmlFor="preview_category">
                Категория
              </label>
              <div>
                <InputForm
                  id="preview_category"
                  value={previewInfo?.category.name}
                  onChange={(e: ChangeEvent<HTMLInputElement>) =>
                    setPreview({
                      ...previewInfo!,
                      category: {
                        ...previewInfo!.category,
                        name: e.target.value,
                      },
                    })
                  }
                ></InputForm>
                {errors.Category ? <div>{errors.Category}</div> : <></>}
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
                    id="preview_price"
                    value={previewInfo?.price}
                    onChange={(e: ChangeEvent<HTMLInputElement>) =>
                      setPreview({
                        ...previewInfo!,
                        price: Number(e.target.value),
                      })
                    }
                  ></InputForm>
                  {errors.Price ? <div>{errors.Price}</div> : <></>}
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
                    <div>{errors.CompleteTime}</div>
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
            onClick={checkPreview}
          >
            посмотреть
          </Button>
        </div>
      </div>
      <div className={classes.line}></div>
      <div className={classes.toggleChanges}>
        <div className={classes.toggleChangeWrapper}>
          <div className={classes.checkboxAndTitle}>
            <Checkbox id="is-program-visible-checkbox" />
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
            <Checkbox id="has-certificates-checkbox" />
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
            <Checkbox id="is-archived-checkbox" />
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
        <Button>сохранить изменения</Button>
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
    </>
  );
}

export default EditPreviewComponent;

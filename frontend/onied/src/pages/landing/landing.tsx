import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import classes from "./landing.module.css";
import CallToActionTemplate, {
  CallToActionData,
} from "../../components/landing/callToActionTemplate/callToActionTemplate";
import StartLearning from "../../assets/startLearning.svg";
import StartTeaching from "../../assets/startTeaching.svg";
import Reviews from "../../components/landing/platformReviews/reviews";
import Slider, { Settings } from "react-slick";
import {
  Dispatch,
  RefObject,
  SetStateAction,
  useEffect,
  useRef,
  useState,
} from "react";
import GeneralCourseCard from "../../components/catalog/courseCards/generalCourseCard";
import CustomArrow from "../../components/landing/customArrow/customArrow";
import { useProfile } from "../../hooks/profile/useProfile";
import api from "../../config/axios";
import CustomBeatLoader from "../../components/general/customBeatLoader";

function Landing() {
  const [profile, _] = useProfile();
  const [userCourses, setUserCourses] = useState<
    Array<CourseCard> | undefined
  >();

  const [mostPopularCourses, setMostPopularCourses] = useState<
    Array<CourseCard> | null | undefined
  >();

  const [recommendedCourses, setRecommendedCourses] = useState<
    Array<CourseCard> | null | undefined
  >();

  const userCoursesSlider = useRef<Slider>(null);
  const mostPopularCoursesSlider = useRef<Slider>(null);
  const recommendedCoursesSlider = useRef<Slider>(null);

  const callToStartLearningData: CallToActionData = {
    imageSrc: StartLearning,
    title: "Учитесь без ограничений",
    text: "Будь то стремление продвигаться по карьерной лестнице, открывать новые  интересы или просто расширять свой кругозор, здесь найдется что-то для вас.  Начните свой образовательный путь уже сегодня!",
    buttonLabel: "начать учиться",
    redirectTo: "/catalog",
  };

  const callToStartTeachingData: CallToActionData = {
    imageSrc: StartTeaching,
    title: "Станьте преподавателем",
    text: "Преподавайте из любой точки мира. OniEd предоставляет вам средства преподавать то, что вы любите.",
    buttonLabel: "начать преподавать",
    redirectTo: "/teaching",
  };

  const defaultSettings: Settings = {
    dots: true,
    arrows: false,
    speed: 400,
    slidesToShow: 5,
    slidesToScroll: 5,
    responsive: [
      {
        breakpoint: 1536,
        settings: {
          slidesToShow: 4,
          slidesToScroll: 4,
          dots: true,
        },
      },
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3,
          dots: true,
        },
      },
    ],
  };

  const [userCoursesSliderSettings, setUserCoursesSliderSettings] =
    useState<Settings>(defaultSettings);
  const [
    mostPopularCoursesSliderSettings,
    setMostPopularCoursesSliderSettings,
  ] = useState<Settings>(defaultSettings);
  const [
    recommendedCoursesSliderSettings,
    setRecommendedCoursesSliderSettings,
  ] = useState<Settings>(defaultSettings);

  const configureSettings = (
    courseCardsAmount: number,
    slider: RefObject<Slider>,
    settingsStateSetter: Dispatch<SetStateAction<Settings>>
  ) => {
    const currentSlidesToShow =
      slider.current?.innerSlider?.track.props.slidesToShow;
    if (courseCardsAmount > currentSlidesToShow) {
      settingsStateSetter((settings) => ({
        ...settings,
        infinite: true,
        dots: true,
        slidesToShow: 5,
        responsive: [
          {
            breakpoint: 1536,
            settings: {
              slidesToShow: 4,
              slidesToScroll: 4,
              dots: true,
              infinite: true,
            },
          },
          {
            breakpoint: 1024,
            settings: {
              slidesToShow: 3,
              slidesToScroll: 3,
              dots: true,
              infinite: true,
            },
          },
        ],
      }));
    } else {
      settingsStateSetter((settings) => ({
        ...settings,
        infinite: false,
        dots: false,
        slidesToScroll: courseCardsAmount,
        responsive: [
          {
            breakpoint: 1536,
            settings: {
              slidesToShow: 4,
              slidesToScroll: courseCardsAmount,
              dots: true,
            },
          },
          {
            breakpoint: 1024,
            settings: {
              slidesToShow: 3,
              slidesToScroll: courseCardsAmount,
              dots: true,
            },
          },
        ],
      }));
    }
  };

  const sliderNext = (slider: RefObject<Slider>) => {
    slider?.current?.slickNext();
  };

  const sliderPrev = (slider: RefObject<Slider>) => {
    slider?.current?.slickPrev();
  };

  useEffect(() => {
    if (profile == null) return;
    api
      .get("/account/courses")
      .then((response) => {
        console.log(response);
        setUserCourses(response.data);
      })
      .catch((error) => {
        console.log(error);
      });
  }, [profile]);

  useEffect(() => {
    api
      .get("/landing/most-popular")
      .then((response) => {
        console.log(response);
        setMostPopularCourses(response.data);
      })
      .catch((error) => {
        console.log(error);
        setMostPopularCourses(null);
      });
  }, []);

  useEffect(() => {
    api
      .get("/landing/recommended")
      .then((response) => {
        console.log(response);
        setRecommendedCourses(response.data);
      })
      .catch((error) => {
        console.log(error);
        setRecommendedCourses(null);
      });
  }, []);

  useEffect(() => {
    if (userCourses === undefined) return;
    configureSettings(
      userCourses!.length,
      userCoursesSlider,
      setUserCoursesSliderSettings
    );
  }, [userCourses]);

  useEffect(() => {
    if (mostPopularCourses == undefined) return;
    configureSettings(
      mostPopularCourses!.length,
      mostPopularCoursesSlider,
      setMostPopularCoursesSliderSettings
    );
  }, [mostPopularCourses]);

  useEffect(() => {
    if (recommendedCourses == undefined) return;
    configureSettings(
      recommendedCourses!.length,
      recommendedCoursesSlider,
      setRecommendedCoursesSliderSettings
    );
  }, [recommendedCourses]);

  return (
    <div className={classes.contentWrapper}>
      {profile == null ? (
        <></>
      ) : userCourses === undefined ? (
        <>
          <h2>Ваши курсы</h2>
          <CustomBeatLoader />
        </>
      ) : (
        <>
          <h2>Ваши курсы</h2>
          <div className={classes.carouselWrapper}>
            <div className={classes.sliderWrapper}>
              {userCoursesSliderSettings.infinite ? (
                <>
                  <CustomArrow
                    onClick={() => sliderPrev(userCoursesSlider)}
                    next={false}
                  />
                  <CustomArrow
                    onClick={() => sliderNext(userCoursesSlider)}
                    next={true}
                  />
                </>
              ) : (
                <></>
              )}

              <Slider ref={userCoursesSlider} {...userCoursesSliderSettings}>
                {userCourses.map((courseCard: CourseCard) => (
                  <div className={classes.sliderItem} key={courseCard.id}>
                    <GeneralCourseCard card={courseCard} />
                  </div>
                ))}
              </Slider>
            </div>
          </div>
        </>
      )}
      {mostPopularCourses === null ? (
        <></>
      ) : (
        <>
          <h2>Самые популярные курсы</h2>
          <div className={classes.carouselWrapper}>
            {mostPopularCourses === undefined ? (
              <CustomBeatLoader />
            ) : (
              <div className={classes.sliderWrapper}>
                {mostPopularCoursesSliderSettings.infinite ? (
                  <>
                    <CustomArrow
                      onClick={() => sliderPrev(mostPopularCoursesSlider)}
                      next={false}
                    />
                    <CustomArrow
                      onClick={() => sliderNext(mostPopularCoursesSlider)}
                      next={true}
                    />
                  </>
                ) : (
                  <></>
                )}

                <Slider
                  ref={mostPopularCoursesSlider}
                  {...mostPopularCoursesSliderSettings}
                >
                  {mostPopularCourses.map((courseCard: CourseCard) => (
                    <div className={classes.sliderItem} key={courseCard.id}>
                      <GeneralCourseCard card={courseCard} />
                    </div>
                  ))}
                </Slider>
              </div>
            )}
          </div>
        </>
      )}

      {recommendedCourses === null ? (
        <></>
      ) : (
        <>
          <h2>Рекомендуемые курсы</h2>
          <div className={classes.carouselWrapper}>
            {recommendedCourses === undefined ? (
              <CustomBeatLoader />
            ) : (
              <div className={classes.sliderWrapper}>
                {recommendedCoursesSliderSettings.infinite ? (
                  <>
                    <CustomArrow
                      onClick={() => sliderPrev(recommendedCoursesSlider)}
                      next={false}
                    />
                    <CustomArrow
                      onClick={() => sliderNext(recommendedCoursesSlider)}
                      next={true}
                    />
                  </>
                ) : (
                  <></>
                )}

                <Slider
                  ref={recommendedCoursesSlider}
                  {...recommendedCoursesSliderSettings}
                >
                  {recommendedCourses.map((courseCard: CourseCard) => (
                    <div className={classes.sliderItem} key={courseCard.id}>
                      <GeneralCourseCard card={courseCard} />
                    </div>
                  ))}
                </Slider>
              </div>
            )}
          </div>
        </>
      )}

      <CallToActionTemplate
        data={callToStartLearningData}
      ></CallToActionTemplate>
      <Reviews></Reviews>
      <CallToActionTemplate
        data={callToStartTeachingData}
      ></CallToActionTemplate>
    </div>
  );
}

export default Landing;

type CourseAuthor = {
  name: string;
};

type CourseCategory = {
  id: string;
  name: string;
};

export type CourseCard = {
  isGlowing: boolean;
  isOwned: boolean;
  pictureHref: string;
  title: string;
  price: number;
  id: string;
  category: CourseCategory;
  author: CourseAuthor;
};

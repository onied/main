import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import classes from "./landing.module.css";
import CallToActionTemplate, {
  CallToActionData,
} from "../../components/landing/callToActionTemplate/callToActionTemplate";
import StartLearning from "../../assets/startLearning.svg";
import StartTeaching from "../../assets/startTeaching.svg";
import Reviews from "../../components/landing/platformReviews/reviews";
import Slider from "react-slick";
import { RefObject, useEffect, useRef, useState } from "react";
import BeatLoader from "react-spinners/BeatLoader";
import {
  tempPopularCourses,
  tempRecommendedCourses,
} from "../../components/landing/temporaryCoursesSource";
import GeneralCourseCard from "../../components/catalog/courseCards/generalCourseCard";
import CustomArrow from "../../components/landing/customArrow/customArrow";
import { useProfile } from "../../hooks/profile/useProfile";
import api from "../../config/axios";

function Landing() {
  const [profile, _] = useProfile();
  const [userCourses, setUserCourses] = useState<
    Array<CourseCard> | undefined
  >();

  const [mostPopularCourses, setMostPopularCourses] = useState<
    Array<CourseCard> | undefined
  >();

  const [recommendedCourses, setRecommendedCourses] = useState<
    Array<CourseCard> | undefined
  >();

  const userCoursesSlider = useRef<Slider>(null);
  const mostPopularCoursesSlider = useRef<Slider>(null);
  const recommendedCoursesSlider = useRef<Slider>(null);

  const [isUserCoursesCarouselInfinite, setIsUserCoursesCarouselInfinite] =
    useState<boolean>(false);

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
    text: "Преподовайте из любой точки мира. OniEd предостовляет вам средства преподавать то, что вы любите.",
    buttonLabel: "начать преподавать",
    redirectTo: "/teaching",
  };

  const testUserCoursesSliderSettings = (
    courseCardsAmount: number,
    slider: RefObject<Slider>
  ) => {
    setIsUserCoursesCarouselInfinite(
      courseCardsAmount > slider.current?.innerSlider?.track.props.slidesToShow
    );
  };

  const [sliderSettings, setSliderSettings] = useState({
    dots: true,
    arrows: false,
    infinite: true,
    speed: 400,
    slidesToShow: 5,
    slidesToScroll: 5,
    responsive: [
      {
        breakpoint: 1536,
        settings: {
          slidesToShow: 4,
          slidesToScroll: 4,
          infinite: true,
          dots: true,
        },
      },
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3,
          infinite: true,
          dots: true,
        },
      },
    ],
  });

  const sliderNext = (slider: RefObject<Slider>) => {
    slider?.current?.slickNext();
  };

  const sliderPrev = (slider: RefObject<Slider>) => {
    slider?.current?.slickPrev();
  };
  useEffect(() => {
    if (profile != null) {
      api
        .get("/account/courses")
        .then((response) => {
          console.log(response);
          setUserCourses(response.data);
          testUserCoursesSliderSettings(
            response.data.length,
            userCoursesSlider
          );
        })
        .catch((error) => {
          console.log(error);
        });
    }

    setTimeout(() => {
      setMostPopularCourses(tempPopularCourses);
      setRecommendedCourses(tempRecommendedCourses);
    }, 700);
  }, []);

  return (
    <div className={classes.contentWrapper}>
      {profile == null || userCourses === undefined ? (
        <></>
      ) : (
        <>
          <h2>Ваши курсы</h2>
          <div className={classes.carouselWrapper}>
            <div className={classes.sliderWrapper}>
              <CustomArrow
                onClick={() => sliderPrev(userCoursesSlider)}
                next={false}
              />
              <Slider
                ref={userCoursesSlider}
                {...{ sliderSettings, infinite: isUserCoursesCarouselInfinite }}
              >
                {userCourses.map((courseCard: CourseCard) => (
                  <div className={classes.sliderItem} key={courseCard.id}>
                    <GeneralCourseCard card={courseCard} owned={true} />
                  </div>
                ))}
              </Slider>
              <CustomArrow
                onClick={() => sliderNext(userCoursesSlider)}
                next={true}
              />
            </div>
          </div>
        </>
      )}
      <h2>Самые популярные курсы</h2>
      <div className={classes.carouselWrapper}>
        {mostPopularCourses === undefined ? (
          <BeatLoader
            cssOverride={{ margin: "30px 30px" }}
            color="var(--accent-color)"
          ></BeatLoader>
        ) : (
          <div className={classes.sliderWrapper}>
            <CustomArrow
              onClick={() => sliderPrev(mostPopularCoursesSlider)}
              next={false}
            />
            <Slider ref={mostPopularCoursesSlider} {...sliderSettings}>
              {mostPopularCourses.map((courseCard: CourseCard) => (
                <div className={classes.sliderItem} key={courseCard.id}>
                  <GeneralCourseCard card={courseCard} owned={false} />
                </div>
              ))}
            </Slider>
            <CustomArrow
              onClick={() => sliderNext(mostPopularCoursesSlider)}
              next={true}
            />
          </div>
        )}
      </div>
      <h2>Рекомендуемые курсы</h2>
      <div className={classes.carouselWrapper}>
        {recommendedCourses === undefined ? (
          <BeatLoader
            cssOverride={{ margin: "30px 30px" }}
            color="var(--accent-color)"
          ></BeatLoader>
        ) : (
          <div className={classes.sliderWrapper}>
            <CustomArrow
              onClick={() => sliderPrev(recommendedCoursesSlider)}
              next={false}
            />
            <Slider ref={recommendedCoursesSlider} {...sliderSettings}>
              {recommendedCourses.map((courseCard: CourseCard) => (
                <div className={classes.sliderItem} key={courseCard.id}>
                  <GeneralCourseCard card={courseCard} owned={false} />
                </div>
              ))}
            </Slider>
            <CustomArrow
              onClick={() => sliderNext(recommendedCoursesSlider)}
              next={true}
            />
          </div>
        )}
      </div>
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
  isGlowing: Boolean;
  pictureHref: string;
  title: string;
  price: number;
  id: string;
  category: CourseCategory;
  author: CourseAuthor;
};

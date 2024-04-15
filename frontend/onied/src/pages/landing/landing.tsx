import classes from "./landing.module.css";
import CallToActionTemplate, {
  CallToActionData,
} from "../../components/landing/callToActionTemplate/callToActionTemplate";
import StartLearning from "../../assets/startLearning.svg";
import StartTeaching from "../../assets/startTeaching.svg";
import Reviews from "../../components/landing/platformReviews/reviews";
import Slider from "react-slick";
import { Carousel } from "nuka-carousel";
import { useEffect, useRef, useState } from "react";
import BeatLoader from "react-spinners/BeatLoader";
import {
  tempPopularCourses,
  tempRecommendedCourses,
} from "../../components/landing/temporaryCoursesSource";
import GeneralCourseCard from "../../components/catalog/courseCards/generalCourseCard";
import CustomArrow from "../../components/landing/customArrow/customArrow";

function Landing() {
  const [mostPopularCourses, setMostPopularCourses] = useState<
    Array<CourseCard> | undefined
  >();

  const [recommendedCourses, setRecommendedCourses] = useState<
    Array<CourseCard> | undefined
  >();

  const slider = useRef<Slider>(null);

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

  const sliderSettings = {
    dots: true,
    arrows: false,
    infinite: true,
    speed: 400,
    slidesToShow: 5,
    slidesToScroll: 5,
  };

  const sliderNext = () => {
    slider?.current?.slickNext();
  };

  const sliderPrev = () => {
    slider?.current?.slickPrev();
  };
  useEffect(() => {
    setTimeout(() => {
      setMostPopularCourses(tempPopularCourses);
      setRecommendedCourses(tempRecommendedCourses);
    }, 700);
  }, []);

  return (
    <div className={classes.contentWrapper}>
      <h2>Самые популярные курсы</h2>
      <div className={classes.carouselWrapper}>
        {mostPopularCourses === undefined ? (
          <BeatLoader
            cssOverride={{ margin: "30px 30px" }}
            color="var(--accent-color)"
          ></BeatLoader>
        ) : (
          <div className={classes.sliderWrapper}>
            <CustomArrow onClick={sliderPrev} next={false} />
            <Slider ref={slider} {...sliderSettings}>
              {mostPopularCourses.map((courseCard: CourseCard) => (
                <div className={classes.sliderItem}>
                  <GeneralCourseCard
                    key={courseCard.id}
                    card={courseCard}
                    owned={false}
                  />
                </div>
              ))}
            </Slider>
            <CustomArrow onClick={sliderNext} next={true} />
          </div>
        )}
      </div>
      <h2>Рекомендуемые курсы</h2>
      <div>
        {recommendedCourses === undefined ? (
          <BeatLoader
            cssOverride={{ margin: "30px 30px" }}
            color="var(--accent-color)"
          ></BeatLoader>
        ) : (
          <Slider {...sliderSettings}>
            {recommendedCourses.map((courseCard: CourseCard) => (
              <div className={classes.sliderItem}>
                <GeneralCourseCard
                  key={courseCard.id}
                  card={courseCard}
                  owned={false}
                />
              </div>
            ))}
          </Slider>
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

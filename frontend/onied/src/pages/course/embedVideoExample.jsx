import Sidebar from "../../components/sidebar/sidebar";
import BlockViewContainer from "../../components/blocks/blockViewContainer";
import EmbedVideo from "../../components/blocks/video/embedVideo";

function CourseEmbedVideo() {
  return (
    <>
      <Sidebar></Sidebar>
      <BlockViewContainer>
        <EmbedVideo href="https://www.youtube.com/watch?v=YfBlwC44gDQ"/>
        <EmbedVideo href="https://vk.com/video-50883936_456243146"/>
        <EmbedVideo href="https://rutube.ru/video/1c69be7b3e28cb58368f69473f6c1d96/?r=wd"/>
      </BlockViewContainer>
    </>
  );
}
export default CourseEmbedVideo;

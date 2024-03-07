import Sidebar from "../../components/sidebar/sidebar";
import BlockViewContainer from "../../components/blocks/blockViewContainer";
import EmbedVideo from "../../components/blocks/video/embedVideo";

function CourseEmbedVideo() {
  return (
    <>
      <Sidebar></Sidebar>
      <BlockViewContainer>
        <EmbedVideo href="https://www.youtube.com/watch?v=YfBlwC44gDQ"/>
        <EmbedVideo href="https://vk.com/video197465133_456239888"/>
      </BlockViewContainer>
    </>
  );
}
export default CourseEmbedVideo;

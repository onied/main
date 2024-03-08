import Sidebar from "../../components/sidebar/sidebar";
import BlockViewContainer from "../../components/blocks/blockViewContainer";
import Summary from "../../components/blocks/summary/summary";
import Tasks from "../../components/blocks/tasks/tasks";
import EmbedVideo from "../../components/blocks/video/embedVideo";
import React, { Fragment } from 'react'
import { Route, Routes } from "react-router-dom";


function Course() {
  return (
    <>
      <Sidebar></Sidebar>
      <BlockViewContainer>
        <Routes>
          <Route path="1" element={Summary()}></Route>
          <Route path="2" element={Tasks()}></Route>
          <Route path="3" element={<Fragment>
            <EmbedVideo href="https://www.youtube.com/watch?v=YfBlwC44gDQ"/>
            <EmbedVideo href="https://vk.com/video-50883936_456243146"/>
            <EmbedVideo href="https://rutube.ru/video/1c69be7b3e28cb58368f69473f6c1d96/?r=wd"/>
            </Fragment>}></Route>
        </Routes>
      </BlockViewContainer>
    </>
  ); // /course/asdf/learn/1 -- summary, /course/asdf/learn/2 -- tasks
}
export default Course;

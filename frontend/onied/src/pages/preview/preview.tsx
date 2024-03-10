import { ReactNode, useState } from 'react'
import Markdown from 'react-markdown';

import classes from './preview.module.css'
import { AllowCertificate } from '../../components/preview/allowCertifiacte/allowCertificate';
import PreviewPicture from '../../components/preview/previewPicture/previewPicture';
import CourseProgram from '../../components/preview/courseProgram/courseProgram';
import Button from '../../components/general/button/button';
import AuthorBlock from '../../components/preview/authorBlock/authorBlock';

type PreviewDto = {
    title: string;
    pictureHref: string,
    description: string;
    hoursCount: number;
    price: number;
    category: {
        id: number;
        name: string;
    }
    courseAuthor: {
        name: string;
        avatarHref: string;
    }
    isArchived: boolean;
    courseProgram: Array<string> | undefined;
};

function Preview(): ReactNode {
    const [dto, setDto] = useState<PreviewDto>(
        {
            title: "preview",
            pictureHref: "https://gas-kvas.com/uploads/posts/2023-02/1676538735_gas-kvas-com-p-vasilii-terkin-detskii-risunok-49.jpg",
            description: "Описание курса. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus. Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.",
            hoursCount: 100,
            price: 3000,
            category: {
                id: 1,
                name: "цифровые технологии"
            },
            courseAuthor: {
                name: "Василий Теркин",
                avatarHref: "https://gas-kvas.com/uploads/posts/2023-02/1676538735_gas-kvas-com-p-vasilii-terkin-detskii-risunok-49.jpg",
            },
            isArchived: true,
            courseProgram: [
                "Модуль 1",
                "Модуль 2",
                "Модуль 3"
            ]
        }
    );

    return (<div className={classes.previewContainer}>
        <div className={classes.previewLeftBlock}>
            <h2 className={classes.previewTitle}>{dto.title}</h2>
            <div className={classes.additionalInfoLine}>
                <a className={classes.additionalInfo}
                   href={`/catalog?=category=${dto.category.id}`}>
                    категория: {dto.category.name}
                </a>
                <span className={classes.additionalInfo}>
                    время прохождения: {dto.hoursCount} часов
                </span>
            </div>
            <Markdown>{dto.description}</Markdown>
            {
                dto.courseProgram === undefined
                ? null
                : <CourseProgram modules={dto.courseProgram} />
            }
        </div>
        <div className={classes.previewRightBlock}>
            <PreviewPicture href={dto.pictureHref} isArchived={dto.isArchived} />
            <h2 className={classes.price}>{dto.price}</h2>
            <Button style={ { width: '100%', fontSize: '20pt'} }>купить</Button>
            <AuthorBlock 
            authorName={dto.courseAuthor.name} 
            authorAvatarHref={dto.courseAuthor.avatarHref}/>
            <AllowCertificate/>
        </div>
    </div>);
}

export default Preview;
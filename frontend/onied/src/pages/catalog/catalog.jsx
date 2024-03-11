import CourseCardsContainer from "../../components/catalog/courseCardsContainer.jsx";
import CatalogHeader from "../../components/catalog/catalogHeader/catalogHeader.jsx";
import CatalogNavigation from "../../components/catalog/catalogNavigation/catalogNavigation.jsx";
import {useEffect, useState} from "react";
import {getAmountOfPages} from "../../components/catalog/catalogPages.js";

function Catalog(){
    const [currentPage, setCurrentPage] = useState(1);

    function onPageChange(pageNumber){
        if (pageNumber < 1){
            setCurrentPage(1);
        }
        else if (pageNumber > getAmountOfPages()){
            setCurrentPage(getAmountOfPages());
        }
        else{
            setCurrentPage(pageNumber);
        }
    }

    useEffect(() => {
        console.log(currentPage);
    }, [currentPage]);

    return(
        <div>
            <CatalogHeader />
            <CourseCardsContainer currentPage={currentPage}/>
            <CatalogNavigation currentPage={currentPage} onPageChange={onPageChange}/>
        </div>
    )
}

export default Catalog;
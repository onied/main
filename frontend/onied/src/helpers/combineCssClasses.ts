const combineCssClasses = (classes: string[]) => classes.filter(cls => cls != null && cls != '').join(' ')

export default combineCssClasses
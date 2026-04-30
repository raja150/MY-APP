 
 

export const pageTitle = (text) =>{
    return `${text} ${process.env.REACT_APP_API_ENV && process.env.REACT_APP_API_ENV} ${process.env.REACT_APP_NAME}`;
}
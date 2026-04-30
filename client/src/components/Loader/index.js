
import React from 'react';
import Loader from 'react-loaders'; 

const Loading = (props) => {
    return <Loader type="line-scale-party" />;

};

export const Saving = (props) => {
    return <Loader type="ball-pulse"
        active style={{ transform: 'scale(0.5)', height:'auto' }} />;

};

export default Loading;
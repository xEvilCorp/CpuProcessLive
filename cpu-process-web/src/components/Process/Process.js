import React from 'react';
import './process.css';

const Process = (props) => {
    const {id, name, ramUsage, cpuUsage} = props.process;
    const sortingMode = props.sortingMode;

    return(
        <div className='processRow'>
            <p className='id'>{id}</p>
            <p className='name'>{name}</p>
            <p className={sortingMode.includes('Ram') ? 'ram sorted' : 'ram'}>{ramUsage.toFixed(1)} MB</p>
            <p className={sortingMode.includes('Cpu') ? 'ram sorted' : 'ram'}>{cpuUsage.toFixed(1)} %</p>
        </div>
    )
};

export default Process;
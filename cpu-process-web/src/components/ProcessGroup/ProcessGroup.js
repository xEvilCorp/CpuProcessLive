import React from 'react';
import './processGroup.css';

const ProcessGroup = (props) => {
    const { name, ramUsage, cpuUsage, items } = props.processGroup;
    const sortingMode = props.sortingMode;
    const toggleItems = param => e => {
        e.target.parentElement.parentElement.parentElement.classList.toggle("active");
    };
    return (
        <div className='processGroup'>
            <button type="button" className="collapsible" onClick={toggleItems()} >
                <div className='processGroupHeader'>
                    <p className='id'>{ramUsage.toFixed(1)}</p>
                    <p className='name'>{name}</p>
                    <p className={sortingMode.includes('Ram') ? 'ram sorted' : 'ram'}>{ramUsage.toFixed(1)} MB</p>
                    <p className={sortingMode.includes('Cpu') ? 'ram sorted' : 'ram'}>{cpuUsage.toFixed(1)} %</p>
                </div>
            </button>
            <div className="processGroupItems">
                <p>Lorem ipsum...</p>
            </div>

        </div>
    )
};

export default ProcessGroup;
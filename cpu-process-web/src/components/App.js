import ProcessList from './ProcessList/ProcessList';
import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import './app.css';


const App = () => {
    const [connection, setConnection] = useState(null);
    const [data, setData] = useState({totalRamUsage: 0, totalCpuUsage: 0,processes: []});
    const [sortingMode, setSortingMode] = useState("CpuDesc");

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:7049/process')
            .withAutomaticReconnect()
            .build();
        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(() => {
                    connection.on('ReceiveData', response => {
                        setData(response);
                    });
                })
                .catch(e => console.log('Connection failed reason: ', e));
        }
    }, [connection]);

    const toggleSortingMode = (column) => {
        let mode = column + "Desc";
        if(sortingMode.includes(mode)) 
        mode = column + "Asc";
        setSortingMode(mode);
    }

    return(
        <div className='App' style={{height: '100%'}}>
            <h1>Computer Process Live</h1>
            <ProcessList 
            data={data} 
            sortingMode={sortingMode} 
            toggleSortingMode={toggleSortingMode} />
        </div>
    )
};

export default App;

/*   */
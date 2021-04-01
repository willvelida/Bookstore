import React from 'react';
import axios from 'axios';

export default class AddBook extends React.Component {
    constructor() {
        super();
        this.state = {
            BookName: '',
            Price: '',
            Category: '',
            Author: ''
        };
    }

    handleChange = event => {
        const field = event.target.id;
        if (field === "BookName") {
            this.setState({ BookName: event.target.value });
        } else if (field === "Price") {
            this.setState({ Price: event.target.value });
        } else if (field === "Category") {
            this.setState({ Category: event.target.value });
        } else if (field === "Author") {
            this.setState({ Author: event.target.value });
        }
    }

    handleSubmit = event => {
        event.preventDefault();
        axios({
            method: "POST",
            url: "https://favelidabookstoreapi.azurewebsites.net/api/Book",
            data: this.state
        }).then((response) => {
            console.log(response);
        });        
    }   

    render() {
        return(
            <div>
                <form onSubmit={this.handleSubmit.bind(this)} method="POST">
                    <label htmlFor="BookName">
                        Book Name:
                        <input 
                        type="text" 
                        id="BookName" 
                        onChange={this.handleChange}
                        required
                         />
                    </label>
                    <label htmlFor="Price">
                        Price:
                        <input 
                        type="number" 
                        id="Price" 
                        step="0.01" 
                        onChange={this.handleChange}
                        required
                         />
                    </label>
                    <label htmlFor="Category">
                        Category:
                        <input 
                        type="text" 
                        id="Category" 
                        onChange={this.handleChange.bind(this)}
                        required
                         />
                    </label>
                    <label htmlFor="Author">
                        Author:
                        <input 
                        type="text" 
                        id="Author" 
                        onChange={this.handleChange.bind(this)}
                        required
                         />
                    </label>
                    <button type="submit">Add</button>
                </form>
            </div>
        )
    }
}
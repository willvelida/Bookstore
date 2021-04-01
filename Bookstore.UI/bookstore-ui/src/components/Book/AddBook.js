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
            <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
                <div className="max-w-md w-full space-y-8">
                    <form 
                        onSubmit={this.handleSubmit.bind(this)} 
                        method="POST"
                        className="mt-8 space-y-6">
                        <label 
                            htmlFor="BookName"
                            className="block text-sm font-medium text-gray-700">
                            Book Name:
                            <input 
                            type="text" 
                            id="BookName" 
                            onChange={this.handleChange}
                            required
                            />
                        </label>
                        <label 
                            htmlFor="Price"
                            className="block text-sm font-medium text-gray-700">
                            Price:
                            <input 
                            type="number" 
                            id="Price" 
                            step="0.01" 
                            onChange={this.handleChange}
                            required
                            />
                        </label>
                        <label 
                            htmlFor="Category"
                            className="block text-sm font-medium text-gray-700">
                            Category:
                            <input 
                            type="text" 
                            id="Category" 
                            onChange={this.handleChange.bind(this)}
                            required
                            />
                        </label>
                        <label 
                            htmlFor="Author"
                            className="block text-sm font-medium text-gray-700">
                            Author:
                            <input 
                            type="text" 
                            id="Author" 
                            onChange={this.handleChange.bind(this)}
                            required
                            />
                        </label>
                        <button 
                            type="submit"
                            className="ml-5 bg-white py-2 px-3 border border-gray-300 rounded-md shadow-sm text-sm leading-4 font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
                            Save
                        </button>
                    </form>
                </div>               
            </div>
        )
    }
}
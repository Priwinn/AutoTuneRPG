#Read csv files in the results folder and make a confidence region plot
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
import os
import sys
import re
import math

# Read the csv files in the results folder
def read_csv_files():
    # Get the current working directory
    cwd = os.getcwd()
    # Get the path to the results folder
    results_path = os.path.join(cwd, 'Assets\Scripts\\results')
    # Get the list of files in the results folder
    files = os.listdir(results_path)
    # Get the csv files in the results folder
    csv_files = [f for f in files if f.endswith('.csv')]
    # Read the csv files into a pandas dataframe
    data = {}
    for file in csv_files:
        # Get the name of the file
        name = re.sub('.csv', '', file)
        # Read the csv file into a pandas dataframe
        data[name] = pd.read_csv(os.path.join(results_path, file))
    return data

def confidence_region_plot(data,index=1):
    # Create an empty figure
    plt.figure()
    name = list(data.values())[0].columns[index]
    # Create a dataframe that has the i-th column of each dataframe in the data dictionary as columns
    df = pd.DataFrame({name: df.iloc[:, index] for name, df in data.items()})
    x = np.arange(0, len(df))
    y = df.mean(axis=1)
    std = df.std(axis=1)
    # Plot the confidence region
    plt.plot(x, y)
    plt.fill_between(x, y - std, y + std, alpha=0.3)
    
    # Set the labels and title
    plt.xlabel('Generation')
    plt.ylabel(name)
        # Save the plot
    plt.savefig(f'confidence_region_plot_{name}.png')
    # Show the plot
    plt.show()



data = read_csv_files()
for i in range(1, len(list(data.values())[0].columns)):
    confidence_region_plot(data, i)
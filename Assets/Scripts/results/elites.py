import os
import json
import csv

# Directory path containing the JSON files
directory = os.path.dirname(os.path.abspath(__file__))

# List to store the JSON data
json_data = []

# Read all JSON files in the directory
for filename in os.listdir(directory):
    if filename.endswith('.json') and filename.startswith('elite'):
        with open(os.path.join(directory, filename)) as file:
            data = json.load(file)
            json_data.append(data)

# Extract keys from the JSON data
keys = set().union(*(data.keys() for data in json_data))

# Create a CSV file and write the JSON data
with open('output.csv', 'w', newline='') as file:
    writer = csv.DictWriter(file, fieldnames=keys)
    writer.writeheader()
    writer.writerows(json_data)
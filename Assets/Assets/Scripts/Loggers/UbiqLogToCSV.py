import sys
import json
import csv
from datetime import datetime
from pathlib import Path

def printAndTerminate():
    print("Usage: UbiqLogToCSV.py filenames...")
    exit(1)

def flatten_dict(d, parent_key='', sep='_'):
    """Recursively flattens a nested dictionary."""
    items = []
    for k, v in d.items():
        new_key = f"{parent_key}{sep}{k}" if parent_key else k
        if isinstance(v, dict):
            items.extend(flatten_dict(v, new_key, sep=sep).items())
        else:
            items.append((new_key.lower(), v))
    return dict(items)

def convert(json_input_file):
    # Load JSON data from file
    with open(json_input_file, "r") as file:
        json_data = json.load(file)
    
    event_data = {}
    general_field_order = []
    special_events = {"Looking At", "Main Camera", "Block Interaction"}
    special_field_orders = {event: [] for event in special_events}
    
    for item in json_data:
        event_type = item.get("event", "unknown")
        if event_type not in event_data:
            event_data[event_type] = []
        
        row = {
            "ticks": item.get("ticks", ''),
            "peer": item.get("peer", ''),
            "event": event_type,
            "datetime": datetime.utcfromtimestamp(item["ticks"] / 10**7 - 62135596800).strftime('%d/%m/%Y %H:%M:%S.%f')[:-3]
        }
        
        if "arg1" in item and isinstance(item["arg1"], dict):
            flattened_args = flatten_dict(item["arg1"])
            row.update(flattened_args)
            
            field_order = special_field_orders.get(event_type, general_field_order)
            for key in flattened_args.keys():
                if key not in field_order:
                    field_order.append(key)
        
        event_data[event_type].append(row)
    
    # Write each event type to a separate CSV file
    for event_type, records in event_data.items():
        records.sort(key=lambda x: x["ticks"])  # Sort records by ascending ticks
        csv_output_file = f"{Path(json_input_file).stem}_{event_type}.csv"
        
        fieldnames = ["ticks", "peer", "event"] + (special_field_orders.get(event_type, general_field_order)) + ["datetime"]
        
        with open(csv_output_file, mode='w', newline='') as csv_file:
            writer = csv.DictWriter(csv_file, fieldnames=fieldnames)
            writer.writeheader()
            writer.writerows(records)
        
        print(f"CSV file '{csv_output_file}' has been created successfully.")

if __name__ == "__main__":
    if len(sys.argv) < 2:
        printAndTerminate()
    
    for filepath in sys.argv[1:]:
        convert(filepath)

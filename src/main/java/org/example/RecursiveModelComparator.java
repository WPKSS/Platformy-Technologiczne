package org.example;

import java.util.Comparator;

public class RecursiveModelComparator implements Comparator<RecursiveModel> {
    @Override
    public int compare(RecursiveModel o1, RecursiveModel o2) {

        int result = Float.compare(o1.getValue(), o2.getValue());

        if (result != 0) {
            return result;
        }

        result = o1.getName().compareTo(o2.getName());

        if (result != 0) {
            return result;
        }

        return Integer.compare(o1.getId(), o2.getId());
    }
}

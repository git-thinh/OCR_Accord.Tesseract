using System;
using System.Collections.Generic;
using System.Text;

namespace ImageOcrExplorer
{
    enum TYPE_FEATURE
    {
        OPEN_FOLDER = 10,
        OPEN_FILES = 11,
        RECENT_OPEN = 12,
        EXIT = 13,

        IMAGE_ORIGIN_SELECTED_ITEM = 50,
        IMAGE_ORIGIN_SELECTED_BINDING = 51,

        FILTER_INSERT_INTO_SCRIPT = 100,
        FILTER_REMOVE_FROM_SCRIPT = 101,
        FILTER_RUN_ALL = 102,
        FILTER_CLEAR_ALL = 103,
        FILTER_EXECUTE_AUTO_CHANGED = 104,
        FILTER_SELECTED_ITEM_EXECUTE = 111,
    }
}
